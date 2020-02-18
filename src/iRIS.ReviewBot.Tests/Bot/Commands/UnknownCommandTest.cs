using System;
using NUnit.Framework;

namespace iRIS.ReviewBot.Tests.Bot.Commands
{
    using iRIS.ReviewBot.Bot.Entities;
    using iRIS.ReviewBot.Bot.Commands;

    public class UnknownCommandTest
    {
        [Test]
        public void UnknownCommand_CheckCommandType()
        {
            var unknownCommand = new UnknownCommand();

            Assert.AreEqual(CommandType.Unknown, unknownCommand.Type);
        }

        [Test]
        public void UnknownCommand_CheckLogic()
        {
            var unknownCommand = new UnknownCommand();
            var context = new CommandContext(null, null, null);

            var result = unknownCommand.Execute(context);
            var data = $"**Неизвестная команда...**{Environment.NewLine}Список доступных команд можно просмотреть отправив **help**";

            Assert.AreEqual(data, result);
        }
    }
}
