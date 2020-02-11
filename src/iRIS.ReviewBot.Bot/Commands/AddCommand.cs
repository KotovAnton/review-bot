using System;
using System.Linq;
using System.Collections.Generic;

namespace iRIS.ReviewBot.Bot.Commands
{
    using iRIS.ReviewBot.Bot.Entities;
    using iRIS.ReviewBot.Bot.Wrappers;

    public class AddCommand : ICommand
    {
        private readonly IDataProvider _dbProvider;
        private readonly IDateTimeWrapper _dateTimeWrapper;
        private readonly IParametersParser _parametersParser;

        public CommandType Type { get; }

        public AddCommand(IDataProvider dbProvider, IDateTimeWrapper dateTimeWrapper, IParametersParser parametersParser)
        {
            _dbProvider = dbProvider;
            _dateTimeWrapper = dateTimeWrapper;
            _parametersParser = parametersParser;

            Type = CommandType.Add;
        }

        public string Execute(CommandContext context)
        {
            var chatMembers = GetChatMembers(context.ChatId);
            var (newReviewerName, newReviewerTeam) = GetCandidateInfo(context);
            var exists = ExistsInChatMembers(chatMembers, newReviewerName);

            if (exists)
            {
                return $"'{newReviewerName}' уже присутствует в списке";
            }
            else
            {
                var newChatMember = GetCandidateForAdd(context.ChatId, newReviewerName, newReviewerTeam);
                AddCandidate(newChatMember);

                var team = string.IsNullOrEmpty(newChatMember.Team) ? string.Empty : $" из '{newChatMember.Team}'";

                return $"'{newChatMember.Name}'{team} добавлен(а) в список";
            }
        }

        private List<ChatMember> GetChatMembers(string chatId)
        {
            return _dbProvider.GetAll(chatId);
        }

        private (string name, string team) GetCandidateInfo(CommandContext context)
        {
            var parsingResult = _parametersParser.ParseNameAndTeam(context.Parameters);
            var name = string.IsNullOrEmpty(parsingResult.Name) ? context.SenderName : parsingResult.Name;
            var team = string.IsNullOrEmpty(parsingResult.Team) ? null : parsingResult.Team;

            return (name, team);
        }

        private ChatMember GetCandidateForAdd(string chatId, string newReviewerName, string newReviewerTeam)
        {
            return new ChatMember
            {
                ChatId = chatId,
                Name = newReviewerName,
                Team = newReviewerTeam,
                LastReview = _dateTimeWrapper.Now
            };
        }

        private bool ExistsInChatMembers(List<ChatMember> chatMembers, string nameOfNewChatMember)
        {
            return chatMembers.Any(_ => string.Equals(_.Name, nameOfNewChatMember, StringComparison.OrdinalIgnoreCase));
        }

        private void AddCandidate(ChatMember newChatMember)
        {
            _dbProvider.Add(newChatMember);
        }
    }
}