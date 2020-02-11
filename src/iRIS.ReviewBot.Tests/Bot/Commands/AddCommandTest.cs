using System;
using System.Linq;
using System.Collections.Generic;
using NUnit.Framework;
using Moq;

namespace iRIS.ReviewBot.Tests.Bot.Commands
{
    using iRIS.ReviewBot.Bot;
    using iRIS.ReviewBot.Bot.Entities;
    using iRIS.ReviewBot.Bot.Commands;
    using iRIS.ReviewBot.Bot.Wrappers;

    public class AddCommandTest
    {
        [Test]
        public void AddCommand_CheckCommandType()
        {
            var addCommand = new AddCommand(null, null, null);

            Assert.AreEqual(addCommand.Type, CommandType.Add);
        }

        [TestCase("", null, "'User Name' добавлен(а) в список")]
        [TestCase("-t Team1", "Team1", "'User Name' из 'Team1' добавлен(а) в список")]
        public void AddCommand_CheckLogic_AddSender(string parameters, string team, string message)
        {
            var dateTimeNow = DateTime.Now;
            var chatMembers = GetChatMembers();
            var dbProviderMock = GetDbProvider(chatMembers);
            var dateTimeWrapperMock = GetDateTimeWrapper(dateTimeNow);
            var parametersParserMock = GetParametersParser();
            var addCommand = new AddCommand(dbProviderMock, dateTimeWrapperMock, parametersParserMock);
            var context = new CommandContext("User Name", "1", parameters);

            var result = addCommand.Execute(context);

            Assert.AreEqual(message, result);
            Assert.AreEqual(6, chatMembers.Count);
            Assert.AreEqual("1", chatMembers[5].ChatId);
            Assert.AreEqual("User Name", chatMembers[5].Name);
            Assert.AreEqual(dateTimeNow, chatMembers[5].LastReview);
            Assert.AreEqual(team, chatMembers[5].Team);
        }

        [TestCase("User Name", "User Name", null, "'User Name' добавлен(а) в список")]
        [TestCase("User Name -t Team1", "User Name", "Team1", "'User Name' из 'Team1' добавлен(а) в список")]
        public void AddCommand_CheckLogic_AddUser(string parameters, string name, string team, string message)
        {
            var dateTimeNow = DateTime.Now;
            var chatMembers = GetChatMembers();
            var dbProviderMock = GetDbProvider(chatMembers);
            var dateTimeWrapperMock = GetDateTimeWrapper(dateTimeNow);
            var parametersParserMock = GetParametersParser();
            var addCommand = new AddCommand(dbProviderMock, dateTimeWrapperMock, parametersParserMock);
            var context = new CommandContext("Sender User Name", "1", parameters);

            var result = addCommand.Execute(context);

            Assert.AreEqual(message, result);
            Assert.AreEqual(6, chatMembers.Count);
            Assert.AreEqual("1", chatMembers[5].ChatId);
            Assert.AreEqual("User Name", chatMembers[5].Name);
            Assert.AreEqual(dateTimeNow, chatMembers[5].LastReview);
            Assert.AreEqual(team, chatMembers[5].Team);
        }

        [Test]
        public void AddCommand_CheckLogic_SenderExists()
        {
            var chatMembers = GetChatMembers();
            var dbProviderMock = GetDbProvider(chatMembers);
            var parametersParserMock = GetParametersParser();
            var addCommand = new AddCommand(dbProviderMock, null, parametersParserMock);
            var context = new CommandContext("User Name_1 From Chat_1", "1", null);

            var result = addCommand.Execute(context);

            Assert.AreEqual(result, "'User Name_1 From Chat_1' уже присутствует в списке");
            Assert.AreEqual(chatMembers.Count, 5);
        }

        [Test]
        public void AddCommand_CheckLogic_UserExists()
        {
            var chatMembers = GetChatMembers();
            var dbProviderMock = GetDbProvider(chatMembers);
            var parametersParserMock = GetParametersParser();
            var addCommand = new AddCommand(dbProviderMock, null, parametersParserMock);
            var context = new CommandContext("Sender Name", "1", "User Name_1 From Chat_1");

            var result = addCommand.Execute(context);

            Assert.AreEqual(result, "'User Name_1 From Chat_1' уже присутствует в списке");
            Assert.AreEqual(chatMembers.Count, 5);
        }

        private IParametersParser GetParametersParser()
        {
            var parametersParser = new Mock<IParametersParser>();
            parametersParser.Setup(_ => _.ParseNameAndTeam("")).Returns(new NameAndTeamParameters("", ""));
            parametersParser.Setup(_ => _.ParseNameAndTeam(null)).Returns(new NameAndTeamParameters("", ""));
            parametersParser.Setup(_ => _.ParseNameAndTeam("-t Team1")).Returns(new NameAndTeamParameters("", "Team1"));
            parametersParser.Setup(_ => _.ParseNameAndTeam("User Name")).Returns(new NameAndTeamParameters("User Name", ""));
            parametersParser.Setup(_ => _.ParseNameAndTeam("User Name -t Team1")).Returns(new NameAndTeamParameters("User Name", "Team1"));
            parametersParser.Setup(_ => _.ParseNameAndTeam("User Name_1 From Chat_1")).Returns(new NameAndTeamParameters("User Name_1 From Chat_1", ""));

            return parametersParser.Object;
        }

        private IDateTimeWrapper GetDateTimeWrapper(DateTime value)
        {
            var dateTimeWrapper = new Mock<IDateTimeWrapper>();
            dateTimeWrapper.Setup(_ => _.Now)
                .Returns(value);

            return dateTimeWrapper.Object;
        }

        private IDataProvider GetDbProvider(List<ChatMember> chatMembers)
        {
            var dbProviderMock = new Mock<IDataProvider>();
            dbProviderMock.Setup(_ => _.Add(It.IsAny<ChatMember>()))
                .Callback<ChatMember>((chatMember) =>
                {
                    chatMembers.Add(chatMember);
                });
            dbProviderMock.Setup(_ => _.GetAll(It.IsAny<string>()))
                .Returns<string>(chatId => chatMembers.Where(x => x.ChatId == chatId).ToList());

            return dbProviderMock.Object;
        }

        private List<ChatMember> GetChatMembers()
        {
            return new List<ChatMember>
            {
                new ChatMember
                {
                    ChatId = "1",
                    Id = 1,
                    Name = "User Name_1 From Chat_1",
                },
                new ChatMember
                {
                    ChatId = "1",
                    Id = 2,
                    Name = "User Name_1 From Chat_1",
                },
                new ChatMember
                {
                    ChatId = "2",
                    Id = 3,
                    Name = "User Name_3 From Chat_2",
                },
                new ChatMember
                {
                    ChatId = "3",
                    Id = 4,
                    Name = "User Name_3 From Chat_3",
                },
                new ChatMember
                {
                    ChatId = "1",
                    Id = 5,
                    Name = "User Name_5 From Chat_1",
                }
            };
        }
    }
}
