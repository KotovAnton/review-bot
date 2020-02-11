using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;

namespace iRIS.ReviewBot.Tests.Bot
{
    using iRIS.ReviewBot.Bot;
    using iRIS.ReviewBot.Bot.Entities;
    using iRIS.ReviewBot.Bot.Commands;

    public class MessageHandlerTest
    {
        [Test]
        public void MessageHandler_Exception()
        {
            var commands = new List<ICommand>();
            var messageHandler = new MessageHandler(commands);
            var messageContext = new MessageData("Sender Name", "Chat Id", "Bot", "Bot bla-bla-bla");

            Assert.Catch<Exception>(() => messageHandler.Processing(messageContext));
        }

        [Test]
        public void MessageHandler_DefaultUnknownCommand()
        {
            var unknownCommandMock = new Mock<ICommand>();
            unknownCommandMock.Setup(_ => _.Type).Returns(CommandType.Unknown);
            unknownCommandMock.Setup(_ => _.Execute(It.IsAny<CommandContext>())).Returns<CommandContext>((_) => "It was unknown command");

            var addCommandMock = new Mock<ICommand>();
            addCommandMock.Setup(_ => _.Type).Returns(CommandType.Add);

            var commands = new List<ICommand>
            {
                addCommandMock.Object,
                unknownCommandMock.Object
            };
            var messageHandler = new MessageHandler(commands);
            var messageContext = new MessageData("Sender Name", "Chat Id", "Bot", "Bot bla-bla-bla");

            var result = messageHandler.Processing(messageContext);
            Assert.AreEqual("It was unknown command", result);
        }

        [Test]
        public void MessageHandler_CheckChoosingCommand()
        {
            var CommandData = default(CommandContext);

            var unknownCommandMock = new Mock<ICommand>();
            unknownCommandMock.Setup(_ => _.Type).Returns(CommandType.Unknown);
            unknownCommandMock.Setup(_ => _.Execute(It.IsAny<CommandContext>())).Returns<CommandContext>((_) => "It was unknown command");

            var addCommandMock = new Mock<ICommand>();
            addCommandMock.Setup(_ => _.Type).Returns(CommandType.Add);
            addCommandMock.Setup(_ => _.Execute(It.IsAny<CommandContext>())).Returns<CommandContext>((_) =>
            {
                CommandData = _;

                return "It was add command";
            });

            var helpCommandMock = new Mock<ICommand>();
            helpCommandMock.Setup(_ => _.Type).Returns(CommandType.Help);
            helpCommandMock.Setup(_ => _.Execute(It.IsAny<CommandContext>())).Returns<CommandContext>((_) => "It was help command");

            var commands = new List<ICommand>
            {
                addCommandMock.Object,
                unknownCommandMock.Object,
                helpCommandMock.Object
            };
            var messageHandler = new MessageHandler(commands);
            var messageContext = new MessageData("Sender Name", "Chat Id", "Bot", "Bot add bla-bla-bla");

            var result = messageHandler.Processing(messageContext);
            Assert.AreEqual("It was add command", result);
            Assert.IsNotNull(CommandData);
            Assert.AreEqual("Chat Id", CommandData.ChatId);
            Assert.AreEqual("bla-bla-bla", CommandData.Parameters);
            Assert.AreEqual("Sender Name", CommandData.SenderName);
        }
    }
}
