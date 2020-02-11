using System.Collections.Generic;
using NUnit.Framework;
using Moq;

namespace iRIS.ReviewBot.Tests.Bot.Commands
{
    using iRIS.ReviewBot.Bot;
    using iRIS.ReviewBot.Bot.Entities;
    using iRIS.ReviewBot.Bot.Commands;

    public class ListCommandTest
    {
        [Test]
        public void ListCommand_CheckCommandType()
        {
            var listCommand = new ListCommand(null);

            Assert.AreEqual(listCommand.Type, CommandType.List);
        }

        [Test]
        public void ListCommand_CheckLogic_ListWithData()
        {
            var chatMembers = new List<ChatMember>
            {
                new ChatMember
                {
                    ChatId = "1",
                    Id = 1,
                    Name = "User1 Name",
                    Team = "Team1"
                },
                new ChatMember
                {
                    ChatId = "1",
                    Id = 2,
                    Name = "User2 Name",
                    Team = "",
                },
                new ChatMember
                {
                    ChatId = "1",
                    Id = 3,
                    Name = "User3 Name",
                    Team = null
                },
                new ChatMember
                {
                    ChatId = "1",
                    Id = 4,
                    Name = "User5 Name",
                    Team = "Team1"
                },
                new ChatMember
                {
                    ChatId = "1",
                    Id = 5,
                    Name = "User4 Name",
                    Team = "Team2"
                }
            };
            var dbProviderMock = GetDbProvider(chatMembers);
            var listCommand = new ListCommand(dbProviderMock);
            var context = new CommandContext(null, null, null);

            var result = listCommand.Execute(context);
            var expectedResult = @"Список:
User1 Name (Team1)
User2 Name
User3 Name
User5 Name (Team1)
User4 Name (Team2)";

            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void ListCommand_CheckLogic_EmptyList()
        {
            var chatMembers = new List<ChatMember>();
            var dbProviderMock = GetDbProvider(chatMembers);
            var listCommand = new ListCommand(dbProviderMock);
            var context = new CommandContext(null, null, null);

            var expectedResult = listCommand.Execute(context);

            Assert.AreEqual(expectedResult, "Список пуст");
        }

        private IDataProvider GetDbProvider(List<ChatMember> chatMembers)
        {
            var dbProviderMock = new Mock<IDataProvider>();
            dbProviderMock.Setup(_ => _.GetAll(It.IsAny<string>()))
                .Returns(chatMembers);

            return dbProviderMock.Object;
        }
    }
}
