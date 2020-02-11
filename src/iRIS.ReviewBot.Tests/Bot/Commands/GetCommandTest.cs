using System;
using System.Collections.Generic;
using NUnit.Framework;
using Moq;

namespace iRIS.ReviewBot.Tests.Bot.Commands
{
    using iRIS.ReviewBot.Bot;
    using iRIS.ReviewBot.Bot.Entities;
    using iRIS.ReviewBot.Bot.Commands;
    using iRIS.ReviewBot.Bot.Wrappers;

    public class GetCommandTest
    {
        [Test]
        public void GetCommand_CheckCommandData()
        {
            var getCommand = new GetCommand(null, null, null, null);

            Assert.AreEqual(getCommand.Type, CommandType.Get);
        }

        [Test]
        public void GetCommand_CheckLogic_EmptyList()
        {
            var chatMembers = new List<ChatMember>();
            var dbProviderMock = GetDbProvider(chatMembers);
            var parametersParser = GetParametersParser();
            var getCommand = new GetCommand(dbProviderMock, null, null, parametersParser);
            var context = new CommandContext("Reviewer1", "1", null);

            var result = getCommand.Execute(context);

            Assert.AreEqual(@"Список ревьюверов пуст

Reviewer1, присоединяйтесь к списку ревьюверов!", result);
        }

        [Test]
        public void GetCommand_CheckLogic_OnlySender()
        {
            var chatMembers = new List<ChatMember>
            {
                new ChatMember
                {
                    ChatId = "1",
                    Id = 1,
                    Name = "Reviewer1"
                }
            };
            var dbProviderMock = GetDbProvider(chatMembers);
            var parametersParser = GetParametersParser();
            var getCommand = new GetCommand(dbProviderMock, null, null, parametersParser);
            var context = new CommandContext("Reviewer1", "1", null);

            var result = getCommand.Execute(context);

            Assert.AreEqual(result, "Список ревьюверов пуст");
        }

        [Test]
        public void GetCommand_CheckLogic_OnlyOneTeam()
        {
            var chatMembers = new List<ChatMember>
            {
                new ChatMember
                {
                    ChatId = "1",
                    Id = 1,
                    Name = "Reviewer1",
                    Team = "Team1"
                },
                new ChatMember
                {
                    ChatId = "1",
                    Id = 3,
                    Name = "Reviewer2",
                    Team = "Team1"
                },
                new ChatMember
                {
                    ChatId = "1",
                    Id = 2,
                    Name = "Reviewer3",
                    Team = "Team1"
                }
            };
            var dbProviderMock = GetDbProvider(chatMembers);
            var parametersParser = GetParametersParser();
            var getCommand = new GetCommand(dbProviderMock, null, null, parametersParser);
            var context = new CommandContext("Reviewer1", "1", null);

            var result = getCommand.Execute(context);

            Assert.AreEqual(result, "Список ревьюверов пуст");
        }

        [Test]
        public void GetCommand_CheckLogic_Offer()
        {
            var nowDateTime = DateTime.Now;
            var chatMembers = GetChatMembers();
            var dbProviderMock = GetDbProvider(chatMembers);
            var randomWrapperMock = GetRandomWrapper(0);
            var dateTimeWrapperMock = GetDateTimeWrapper(nowDateTime);
            var parametersParser = GetParametersParser();
            var getCommand = new GetCommand(dbProviderMock, randomWrapperMock, dateTimeWrapperMock, parametersParser);
            var context = new CommandContext("Reviewer100", "1", null);

            var result = getCommand.Execute(context);

            Assert.AreEqual(@"Reviewer0

Reviewer100, присоединяйтесь к списку ревьюверов!", result);
            Assert.AreEqual(chatMembers[0].LastReview, nowDateTime);
            Assert.AreNotEqual(chatMembers[1].LastReview, nowDateTime);
            Assert.AreNotEqual(chatMembers[2].LastReview, nowDateTime);
            Assert.AreNotEqual(chatMembers[3].LastReview, nowDateTime);
            Assert.AreNotEqual(chatMembers[4].LastReview, nowDateTime);
            Assert.AreNotEqual(chatMembers[5].LastReview, nowDateTime);
            Assert.AreNotEqual(chatMembers[6].LastReview, nowDateTime);
            Assert.AreNotEqual(chatMembers[7].LastReview, nowDateTime);
            Assert.AreNotEqual(chatMembers[8].LastReview, nowDateTime);
            Assert.AreNotEqual(chatMembers[9].LastReview, nowDateTime);
            Assert.AreNotEqual(chatMembers[10].LastReview, nowDateTime);
        }

        [TestCase(0, 6, "Reviewer6")]
        [TestCase(2, 8, "Reviewer8")]
        public void GetCommand_CheckLogic_DefaultCount(int randomValue, int id, string expectedResult)
        {
            var nowDateTime = DateTime.Now;
            var chatMembers = GetChatMembers();
            var dbProviderMock = GetDbProvider(chatMembers);
            var randomWrapperMock = GetRandomWrapper(randomValue);
            var dateTimeWrapperMock = GetDateTimeWrapper(nowDateTime);
            var parametersParser = GetParametersParser();
            var getCommand = new GetCommand(dbProviderMock, randomWrapperMock, dateTimeWrapperMock, parametersParser);
            var context = new CommandContext("Reviewer0", "1", null);

            var result = getCommand.Execute(context);

            Assert.AreEqual(expectedResult, result);
            Assert.AreEqual(nowDateTime, chatMembers[id].LastReview);
        }

        [Test]
        public void GetCommand_CheckLogic_SenderWithoutTeam()
        {
            var nowDateTime = DateTime.Now;
            var chatMembers = GetChatMembers();
            var dbProviderMock = GetDbProvider(chatMembers);
            var randomWrapperMock = GetRandomWrapper(2);
            var dateTimeWrapperMock = GetDateTimeWrapper(nowDateTime);
            var parametersParser = GetParametersParser();
            var getCommand = new GetCommand(dbProviderMock, randomWrapperMock, dateTimeWrapperMock, parametersParser);
            var context = new CommandContext("Reviewer4", "1", null);

            var result = getCommand.Execute(context);

            Assert.AreEqual("Reviewer10", result);
            Assert.AreEqual(nowDateTime, chatMembers[10].LastReview);
        }

        [Test]
        public void GetCommand_CheckLogic_CustomCount()
        {
            var nowDateTime = DateTime.Now;
            var chatMembers = GetChatMembers();
            var dbProviderMock = GetDbProvider(chatMembers);
            var randomWrapperMock = GetRandomWrapper(1);
            var dateTimeWrapperMock = GetDateTimeWrapper(nowDateTime);
            var parametersParser = GetParametersParser();
            var getCommand = new GetCommand(dbProviderMock, randomWrapperMock, dateTimeWrapperMock, parametersParser);
            var context = new CommandContext("Reviewer0", "1", "2");

            var result = getCommand.Execute(context);

            Assert.AreEqual(result, @"Reviewer10
Reviewer8");
            Assert.AreNotEqual(chatMembers[0].LastReview, nowDateTime);
            Assert.AreNotEqual(chatMembers[1].LastReview, nowDateTime);
            Assert.AreNotEqual(chatMembers[2].LastReview, nowDateTime);
            Assert.AreNotEqual(chatMembers[3].LastReview, nowDateTime);
            Assert.AreNotEqual(chatMembers[4].LastReview, nowDateTime);
            Assert.AreNotEqual(chatMembers[5].LastReview, nowDateTime);
            Assert.AreNotEqual(chatMembers[6].LastReview, nowDateTime);
            Assert.AreNotEqual(chatMembers[7].LastReview, nowDateTime);
            Assert.AreEqual(chatMembers[8].LastReview, nowDateTime);
            Assert.AreNotEqual(chatMembers[9].LastReview, nowDateTime);
            Assert.AreEqual(chatMembers[10].LastReview, nowDateTime);
        }

        [Test]
        public void GetCommand_CheckLogic_BigCustomCount_WithoutTeam()
        {
            var nowDateTime = DateTime.Now;
            var chatMembers = GetChatMembers();
            var dbProviderMock = GetDbProvider(chatMembers);
            var randomWrapperMock = GetRandomWrapper(0);
            var dateTimeWrapperMock = GetDateTimeWrapper(nowDateTime);
            var parametersParser = GetParametersParser();
            var getCommand = new GetCommand(dbProviderMock, randomWrapperMock, dateTimeWrapperMock, parametersParser);
            var context = new CommandContext("Reviewer0", "1", "100");

            var result = getCommand.Execute(context);

            Assert.AreEqual(result, @"Reviewer6
Reviewer10
Reviewer8
Reviewer1
Reviewer7
Reviewer4
Reviewer9
Reviewer2");
            Assert.AreNotEqual(chatMembers[0].LastReview, nowDateTime);
            Assert.AreEqual(chatMembers[1].LastReview, nowDateTime);
            Assert.AreEqual(chatMembers[2].LastReview, nowDateTime);
            Assert.AreNotEqual(chatMembers[3].LastReview, nowDateTime);
            Assert.AreEqual(chatMembers[4].LastReview, nowDateTime);
            Assert.AreNotEqual(chatMembers[5].LastReview, nowDateTime);
            Assert.AreEqual(chatMembers[6].LastReview, nowDateTime);
            Assert.AreEqual(chatMembers[7].LastReview, nowDateTime);
            Assert.AreEqual(chatMembers[8].LastReview, nowDateTime);
            Assert.AreEqual(chatMembers[9].LastReview, nowDateTime);
            Assert.AreEqual(chatMembers[10].LastReview, nowDateTime);
        }

        [Test]
        public void GetCommand_CheckLogic_BigCustomCount_SenderTeam()
        {
            var nowDateTime = DateTime.Now;
            var chatMembers = GetChatMembers();
            var dbProviderMock = GetDbProvider(chatMembers);
            var randomWrapperMock = GetRandomWrapper(0);
            var dateTimeWrapperMock = GetDateTimeWrapper(nowDateTime);
            var parametersParser = GetParametersParser();
            var getCommand = new GetCommand(dbProviderMock, randomWrapperMock, dateTimeWrapperMock, parametersParser);
            var context = new CommandContext("Reviewer0", "1", "100 -t Team0");

            var result = getCommand.Execute(context);

            Assert.AreEqual(result, @"Reviewer3
Reviewer5");
            Assert.AreNotEqual(chatMembers[0].LastReview, nowDateTime);
            Assert.AreNotEqual(chatMembers[1].LastReview, nowDateTime);
            Assert.AreNotEqual(chatMembers[2].LastReview, nowDateTime);
            Assert.AreEqual(chatMembers[3].LastReview, nowDateTime);
            Assert.AreNotEqual(chatMembers[4].LastReview, nowDateTime);
            Assert.AreEqual(chatMembers[5].LastReview, nowDateTime);
            Assert.AreNotEqual(chatMembers[6].LastReview, nowDateTime);
            Assert.AreNotEqual(chatMembers[7].LastReview, nowDateTime);
            Assert.AreNotEqual(chatMembers[8].LastReview, nowDateTime);
            Assert.AreNotEqual(chatMembers[9].LastReview, nowDateTime);
            Assert.AreNotEqual(chatMembers[10].LastReview, nowDateTime);
        }

        [Test]
        public void GetCommand_CheckLogic_BigCustomCount_TargetTeam()
        {
            var nowDateTime = DateTime.Now;
            var chatMembers = GetChatMembers();
            var dbProviderMock = GetDbProvider(chatMembers);
            var randomWrapperMock = GetRandomWrapper(0);
            var dateTimeWrapperMock = GetDateTimeWrapper(nowDateTime);
            var parametersParser = GetParametersParser();
            var getCommand = new GetCommand(dbProviderMock, randomWrapperMock, dateTimeWrapperMock, parametersParser);
            var context = new CommandContext("Reviewer0", "1", "100 -t Team1");

            var result = getCommand.Execute(context);

            Assert.AreEqual(result, @"Reviewer6
Reviewer10
Reviewer1
Reviewer7");
            Assert.AreNotEqual(chatMembers[0].LastReview, nowDateTime);
            Assert.AreEqual(chatMembers[1].LastReview, nowDateTime);
            Assert.AreNotEqual(chatMembers[2].LastReview, nowDateTime);
            Assert.AreNotEqual(chatMembers[3].LastReview, nowDateTime);
            Assert.AreNotEqual(chatMembers[4].LastReview, nowDateTime);
            Assert.AreNotEqual(chatMembers[5].LastReview, nowDateTime);
            Assert.AreEqual(chatMembers[6].LastReview, nowDateTime);
            Assert.AreEqual(chatMembers[7].LastReview, nowDateTime);
            Assert.AreNotEqual(chatMembers[8].LastReview, nowDateTime);
            Assert.AreNotEqual(chatMembers[9].LastReview, nowDateTime);
            Assert.AreEqual(chatMembers[10].LastReview, nowDateTime);
        }

        [Test]
        public void GetCommand_CheckLogic_SenderWithoutTeam_BigCustomCount()
        {
            var nowDateTime = DateTime.Now;
            var chatMembers = GetChatMembers();
            var dbProviderMock = GetDbProvider(chatMembers);
            var randomWrapperMock = GetRandomWrapper(0);
            var dateTimeWrapperMock = GetDateTimeWrapper(nowDateTime);
            var parametersParser = GetParametersParser();
            var getCommand = new GetCommand(dbProviderMock, randomWrapperMock, dateTimeWrapperMock, parametersParser);
            var context = new CommandContext("Reviewer4", "1", "100");

            var result = getCommand.Execute(context);

            Assert.AreEqual(result, @"Reviewer0
Reviewer6
Reviewer10
Reviewer3
Reviewer8
Reviewer5
Reviewer1
Reviewer7
Reviewer9
Reviewer2");
            Assert.AreEqual(chatMembers[0].LastReview, nowDateTime);
            Assert.AreEqual(chatMembers[1].LastReview, nowDateTime);
            Assert.AreEqual(chatMembers[2].LastReview, nowDateTime);
            Assert.AreEqual(chatMembers[3].LastReview, nowDateTime);
            Assert.AreNotEqual(chatMembers[4].LastReview, nowDateTime);
            Assert.AreEqual(chatMembers[5].LastReview, nowDateTime);
            Assert.AreEqual(chatMembers[6].LastReview, nowDateTime);
            Assert.AreEqual(chatMembers[7].LastReview, nowDateTime);
            Assert.AreEqual(chatMembers[8].LastReview, nowDateTime);
            Assert.AreEqual(chatMembers[9].LastReview, nowDateTime);
            Assert.AreEqual(chatMembers[10].LastReview, nowDateTime);
        }

        private IDataProvider GetDbProvider(List<ChatMember> chatMembers)
        {
            var dbProviderMock = new Mock<IDataProvider>();
            dbProviderMock.Setup(_ => _.GetAll(It.IsAny<string>()))
                .Returns(chatMembers);

            return dbProviderMock.Object;
        }

        private IRandomWrapper GetRandomWrapper(int value)
        {
            var randomWrapperMock = new Mock<IRandomWrapper>();
            randomWrapperMock.Setup(_ => _.Next(It.IsAny<int>()))
                .Returns(value);

            return randomWrapperMock.Object;
        }

        private IDateTimeWrapper GetDateTimeWrapper(DateTime value)
        {
            var dateTimeWrapper = new Mock<IDateTimeWrapper>();
            dateTimeWrapper.Setup(_ => _.Now)
                .Returns(value);

            return dateTimeWrapper.Object;
        }
        
        private IParametersParser GetParametersParser()
        {
            var parametersParser = new Mock<IParametersParser>();
            parametersParser.Setup(_ => _.ParseCountAndTeam("")).Returns(new CountAndTeamParameters(1, ""));
            parametersParser.Setup(_ => _.ParseCountAndTeam(null)).Returns(new CountAndTeamParameters(1, ""));
            parametersParser.Setup(_ => _.ParseCountAndTeam("-t Team1")).Returns(new CountAndTeamParameters(1, "Team1"));
            parametersParser.Setup(_ => _.ParseCountAndTeam("100")).Returns(new CountAndTeamParameters(100, ""));
            parametersParser.Setup(_ => _.ParseCountAndTeam("100 -t Team0")).Returns(new CountAndTeamParameters(100, "Team0"));
            parametersParser.Setup(_ => _.ParseCountAndTeam("100 -t Team1")).Returns(new CountAndTeamParameters(100, "Team1"));
            parametersParser.Setup(_ => _.ParseCountAndTeam("2")).Returns(new CountAndTeamParameters(2, ""));
            parametersParser.Setup(_ => _.ParseCountAndTeam("User Name_1 From Chat_1")).Returns(new CountAndTeamParameters(1, ""));

            return parametersParser.Object;
        }

        private List<ChatMember> GetChatMembers()
        {
            return new List<ChatMember>
            {
                new ChatMember
                {
                    ChatId = "1",
                    Id = 1,
                    Name = "Reviewer0",
                    Team = "Team0",
                    LastReview = DateTime.Now.AddDays(-100),
                },
                new ChatMember
                {
                    ChatId = "1",
                    Id = 2,
                    Name = "Reviewer1",
                    Team = "Team1",
                    LastReview = DateTime.Now.AddDays(-5),
                },
                new ChatMember
                {
                    ChatId = "1",
                    Id = 3,
                    Name = "Reviewer2",
                    Team = "Team2",
                    LastReview = DateTime.Now.AddDays(-1),
                },
                new ChatMember
                {
                    ChatId = "1",
                    Id = 4,
                    Name = "Reviewer3",
                    Team = "Team0",
                    LastReview = DateTime.Now.AddDays(-8),
                },
                new ChatMember
                {
                    ChatId = "1",
                    Id = 5,
                    Name = "Reviewer4",
                    Team = null,
                    LastReview = DateTime.Now.AddDays(-3),
                },
                new ChatMember
                {
                    ChatId = "1",
                    Id = 6,
                    Name = "Reviewer5",
                    Team = "Team0",
                    LastReview = DateTime.Now.AddDays(-6),
                },
                new ChatMember
                {
                    ChatId = "1",
                    Id = 7,
                    Name = "Reviewer6",
                    Team = "Team1",
                    LastReview = DateTime.Now.AddDays(-10),
                },
                new ChatMember
                {
                    ChatId = "1",
                    Id = 8,
                    Name = "Reviewer7",
                    Team = "Team1",
                    LastReview = DateTime.Now.AddDays(-4),
                },
                new ChatMember
                {
                    ChatId = "1",
                    Id = 9,
                    Name = "Reviewer8",
                    Team = "Team2",
                    LastReview = DateTime.Now.AddDays(-7),
                },
                new ChatMember
                {
                    ChatId = "1",
                    Id = 10,
                    Name = "Reviewer9",
                    Team = "Team2",
                    LastReview = DateTime.Now.AddDays(-2),
                },
                new ChatMember
                {
                    ChatId = "1",
                    Id = 11,
                    Name = "Reviewer10",
                    Team = "Team1",
                    LastReview = DateTime.Now.AddDays(-9),
                }
            };
        }
    }
}
