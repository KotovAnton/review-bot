using System.Collections.Generic;
using NUnit.Framework;
using Moq;

namespace iRIS.ReviewBot.Tests.Bot.Commands
{
    using iRIS.ReviewBot.Bot;
    using iRIS.ReviewBot.Bot.Entities;
    using iRIS.ReviewBot.Bot.Commands;

    public class DeleteCommandTest
    {
        [Test]
        public void DeleteCommand_CheckCommandType()
        {
            var deleteCommand = new DeleteCommand(null);

            Assert.AreEqual(deleteCommand.Type, CommandType.Delete);
        }

        [Test]
        public void DeleteCommand_CheckLogic_DeleteSender()
        {
            var chatMembers = new List<ChatMember>
            {
                new ChatMember
                {
                    ChatId = "1",
                    Id = 1,
                    Name = "Any User Name"
                },
                new ChatMember
                {
                    ChatId = "1",
                    Id = 2,
                    Name = "User Name"
                }
            };
            var dbProviderMock = GetDbProvider(chatMembers);
            var deleteCommand = new DeleteCommand(dbProviderMock);
            var context = new CommandContext("User Name", "1", null);

            var result = deleteCommand.Execute(context);

            Assert.AreEqual(result, "'User Name' удален(а) из списка");
            Assert.AreEqual(chatMembers.Count, 1);
            Assert.AreEqual(chatMembers[0].Name, "Any User Name");
        }

        [Test]
        public void DeleteCommand_CheckLogic_DeleteUser()
        {
            var chatMembers = new List<ChatMember>
            {
                new ChatMember
                {
                    ChatId = "1",
                    Id = 1,
                    Name = "Sender User Name"
                },
                new ChatMember
                {
                    ChatId = "1",
                    Id = 2,
                    Name = "User Name"
                }
            };
            var dbProviderMock = GetDbProvider(chatMembers);
            var deleteCommand = new DeleteCommand(dbProviderMock);
            var context = new CommandContext("Sender User Name", "1", "User Name");

            var result = deleteCommand.Execute(context);

            Assert.AreEqual(result, "'User Name' удален(а) из списка");
            Assert.AreEqual(chatMembers.Count, 1);
            Assert.AreEqual(chatMembers[0].Name, "Sender User Name");
        }

        [Test]
        public void DeleteCommand_CheckLogic_SenderNotExists()
        {
            var chatMembers = new List<ChatMember>
            {
                new ChatMember
                {
                    ChatId = "1",
                    Id = 1,
                    Name = "Any User Name"
                },
                new ChatMember
                {
                    ChatId = "1",
                    Id = 2,
                    Name = "Super User Name"
                }
            };
            var dbProviderMock = GetDbProvider(chatMembers);
            var deleteCommand = new DeleteCommand(dbProviderMock);
            var context = new CommandContext("User Name", "1", null);

            var result = deleteCommand.Execute(context);

            Assert.AreEqual(result, "'User Name' отсутствует в списке");
            Assert.AreEqual(chatMembers.Count, 2);
        }

        [Test]
        public void DeleteCommand_CheckLogic_UserNotExists()
        {
            var chatMembers = new List<ChatMember>
            {
                new ChatMember
                {
                    ChatId = "1",
                    Id = 1,
                    Name = "Any User Name"
                },
                new ChatMember
                {
                    ChatId = "1",
                    Id = 2,
                    Name = "Super User Name"
                }
            };
            var dbProviderMock = GetDbProvider(chatMembers);
            var deleteCommand = new DeleteCommand(dbProviderMock);
            var context = new CommandContext("Sender User Name", "1", "User Name");

            var result = deleteCommand.Execute(context);

            Assert.AreEqual(result, "'User Name' отсутствует в списке");
            Assert.AreEqual(chatMembers.Count, 2);
        }

        private IDataProvider GetDbProvider(List<ChatMember> chatMembers)
        {
            var dbProviderMock = new Mock<IDataProvider>();
            dbProviderMock.Setup(_ => _.GetAll(It.IsAny<string>()))
                .Returns(chatMembers);
            dbProviderMock.Setup(_ => _.Delete(It.IsAny<ChatMember>()))
                .Callback<ChatMember>((chatMember) => chatMembers.Remove(chatMember));

            return dbProviderMock.Object;
        }

    }
}
