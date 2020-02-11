using System;
using System.Linq;
using System.Collections.Generic;

namespace iRIS.ReviewBot.Bot
{
    using iRIS.ReviewBot.Bot.Entities;
    using iRIS.ReviewBot.Bot.Commands;

    public class MessageHandler : IMessageHandler
    {
        private readonly List<ICommand> _commands;

        public MessageHandler(List<ICommand> commands)
        {
            _commands = commands;
        }

        public string Processing(MessageData messageContext)
        {
            var (commandName, commandParam) = ParseMessage(messageContext);
            var command = GetCommand(commandName);
            var commandContext = GetCommandContext(messageContext, commandParam);

            return command.Execute(commandContext);
        }

        private (string commandName, string commandParam) ParseMessage(MessageData messageContext)
        {
            var messageText = messageContext.MessageText.Replace(messageContext.RecipientName, string.Empty).Trim();

            if (messageText.Contains(' '))
            {
                var firstSpaceIndex =  messageText.IndexOf(' ');
                var commandName = messageText.Substring(0, firstSpaceIndex).Trim();
                var commandParam = messageText.Substring(firstSpaceIndex).Trim();

                return (commandName, commandParam);
            }
            else
            {
                return (messageText, string.Empty);
            }
        }

        private ICommand GetCommand(string messageCommand)
        {
            var command = _commands.FirstOrDefault(_ => string.Equals(_.Type.ToString(), messageCommand, StringComparison.OrdinalIgnoreCase))
                ?? _commands.FirstOrDefault(_ => _.Type == CommandType.Unknown)
                ?? throw new Exception($"Unknown command '{messageCommand}' and did not set command with type'{CommandType.Unknown}'");

            return command;
        }

        private CommandContext GetCommandContext(MessageData messageContext, string commandParam)
        {
            return new CommandContext(messageContext.SenderName, messageContext.ChatId, commandParam);
        }
    }
}