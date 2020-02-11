using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace iRIS.ReviewBot.Bot.Commands
{
    using iRIS.ReviewBot.Bot.Entities;

    public class ListCommand : ICommand
    {
        private readonly IDataProvider _dbProvider;

        public CommandType Type { get; }

        public ListCommand(IDataProvider dbProvider)
        {
            _dbProvider = dbProvider;

            Type = CommandType.List;
        }

        public string Execute(CommandContext context)
        {
            var chatMembers = GetChatMembers(context.ChatId);

            if (chatMembers.Any())
            {
                return GetList(chatMembers);
            }
            else
            {
                return "Список пуст";
            }
        }

        private List<ChatMember> GetChatMembers(string chatId)
        {
            return _dbProvider.GetAll(chatId);
        }

        private string GetList(List<ChatMember> chatMembers)
        {
            var sb = new StringBuilder();

            sb.Append("Список:");

            foreach (var chatMember in chatMembers)
            {
                sb.AppendLine();
                sb.Append(chatMember.Name);

                if(!string.IsNullOrEmpty(chatMember.Team))
                {
                    sb.Append($" ({chatMember.Team})");
                }
            }

            return sb.ToString();
        }
    }
}
