using System;
using System.Linq;

namespace iRIS.ReviewBot.Bot.Commands
{
    using iRIS.ReviewBot.Bot.Entities;

    public class DeleteCommand : ICommand
    {
        private readonly IDataProvider _dbProvider;

        public CommandType Type { get; }

        public DeleteCommand(IDataProvider dbProvider)
        {
            _dbProvider = dbProvider;

            Type = CommandType.Delete;
        }

        public string Execute(CommandContext context)
        {
            var chatMemberName = GetChatMemberName(context);
            var chatMember = GetChatMember(context.ChatId, chatMemberName);

            if (chatMember != null)
            {
                Delete(chatMember);

                return $"'{chatMemberName}' удален(а) из списка";
            }
            else
            {
                return $"'{chatMemberName}' отсутствует в списке";
            }
        }

        private string GetChatMemberName(CommandContext context)
        {
            return string.IsNullOrEmpty(context.Parameters) ? context.SenderName : context.Parameters;
        }

        private ChatMember GetChatMember(string chatId, string name)
        {
            return _dbProvider.GetAll(chatId)
                .FirstOrDefault(_ => string.Equals(_.Name, name, StringComparison.OrdinalIgnoreCase));
        }

        private void Delete(ChatMember candidateForDeletion)
        {
            _dbProvider.Delete(candidateForDeletion);
        }
    }
}
