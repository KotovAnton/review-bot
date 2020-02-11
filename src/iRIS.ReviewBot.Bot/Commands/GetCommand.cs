using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;

namespace iRIS.ReviewBot.Bot.Commands
{
    using iRIS.ReviewBot.Bot.Wrappers;
    using iRIS.ReviewBot.Bot.Entities;

    public class GetCommand : ICommand
    {
        private readonly IDataProvider _dbProvider;
        private readonly IRandomWrapper _randomWrapper;
        private readonly IDateTimeWrapper _dateTimeWrapper;
        private readonly IParametersParser _parametersParser;

        public CommandType Type { get; }

        public GetCommand(IDataProvider dbProvider, IRandomWrapper randomWrapper, IDateTimeWrapper dateTimeWrapper, IParametersParser parametersParser)
        {
            _dbProvider = dbProvider;
            _randomWrapper = randomWrapper;
            _dateTimeWrapper = dateTimeWrapper;
            _parametersParser = parametersParser;

            Type = CommandType.Get;
        }

        public string Execute(CommandContext context)
        {
            var chatMembers = _dbProvider.GetAll(context.ChatId);
            var parameters = _parametersParser.ParseCountAndTeam(context.Parameters);
            var filteredChatMemabers = FilterBySender(chatMembers, context.SenderName, parameters.Team);
            var countOfCandidates = GetCountOfCandidates(filteredChatMemabers.Count, parameters.Count);
            var candidates = GetCandidates(filteredChatMemabers, countOfCandidates);
            var senderExists = SenderExists(chatMembers, context.SenderName);
            var result = new StringBuilder();

            if (candidates.Any())
            {
                var victims = GetVictims(candidates, parameters.Count);

                UpdateChatMembers(victims);

                result.Append(string.Join(Environment.NewLine, victims.Select(_ => _.Name)));
            }
            else
            {
                result.Append("Список ревьюверов пуст");
            }

            if (!senderExists)
            {
                result.AppendLine();
                result.AppendLine();
                result.Append($"{context.SenderName}, присоединяйтесь к списку ревьюверов!");
            }

            return result.ToString();
        }

        private List<ChatMember> FilterBySender(List<ChatMember> chatMembers, string senderName, string team)
        {
            var sender = chatMembers.FirstOrDefault(_ => string.Equals(_.Name, senderName, StringComparison.OrdinalIgnoreCase));

            Func<ChatMember, bool> defaultTrue = (_) => { return true; };
            Func<ChatMember, bool> filterBySenderId =  null;
            Func<ChatMember, bool> filterByTeam = null;
            
            if (sender != null)
            {
                filterBySenderId = (_) => { return _.Id != sender.Id; };
            }
            else
            {
                filterBySenderId = defaultTrue;
            }

            if(!string.IsNullOrEmpty(team))
            {
                filterByTeam = (_) => { return string.Equals(_.Team, team, StringComparison.OrdinalIgnoreCase); };
            }
            else if(!string.IsNullOrEmpty(sender?.Team))
            {
                filterByTeam = (_) => { return !string.Equals(_.Team, sender.Team, StringComparison.OrdinalIgnoreCase); };
            }
            else
            {
                filterByTeam = defaultTrue;
            }

            return chatMembers = chatMembers
                    .Where(_ => filterBySenderId(_) && filterByTeam(_))
                    .ToList();
        }

        private List<ChatMember> GetCandidates(List<ChatMember> chatMembers, int countOfCandidates)
        {
            var candidates = chatMembers
                .OrderBy(_ => _.LastReview)
                .Take(countOfCandidates)
                .ToList();

            return candidates;
        }

        private int GetCountOfCandidates(int membersCount, int reviewersCount)
        {
            int candidatesCount;

            switch (reviewersCount)
            {
                case 1: candidatesCount = 3; break;
                case 2: candidatesCount = 4; break;
                case 3: 
                case 4: candidatesCount = 5; break;
                default: candidatesCount = reviewersCount; break; 
            }

            return Math.Min(candidatesCount, membersCount);
        }

        private List<ChatMember> GetVictims(List<ChatMember> chatMembers, int count)
        {
            var result = new List<ChatMember>();

            while (chatMembers.Any() && count > 0)
            {
                var randomIndex = _randomWrapper.Next(chatMembers.Count);
                var victim = chatMembers[randomIndex];

                result.Add(victim);
                chatMembers.RemoveAt(randomIndex);

                count--;
            }

            return result;
        }

        private void UpdateChatMembers(List<ChatMember> chatMembers)
        {
            foreach (var chatMember in chatMembers)
            {
                chatMember.LastReview = _dateTimeWrapper.Now;
                _dbProvider.Update(chatMember);
            }
        }

        private bool SenderExists(List<ChatMember> chatMembers, string senderName)
        {
            return chatMembers.Any(_ => string.Equals(_.Name, senderName, StringComparison.OrdinalIgnoreCase));
        }
    }
}
