using System;
using NUnit.Framework;

namespace iRIS.ReviewBot.Tests.Bot
{
    using iRIS.ReviewBot.Bot;

    public class ParametersParserTest
    {
        [TestCase(null, "", "")]
        [TestCase("", "", "")]
        [TestCase("       ", "", "")]
        [TestCase("User name -t Team1","User name","Team1")]
        [TestCase("User Name -to Team1", "User Name -to Team1", "")]
        [TestCase("User with   long name  -t    Team with long   name", "User with   long name", "Team with long   name")]
        [TestCase("   Only user with   long name", "Only user with   long name", "")]
        [TestCase("-t Only team with   long name   ", "", "Only team with   long name")]
        public void NameAndTeam_Parsing_CheckLogic(string text, string name, string team)
        {
            var result = new ParametersParser().ParseNameAndTeam(text);

            Assert.AreEqual(result.Name, name);
            Assert.AreEqual(result.Team, team);
        }

        [TestCase("User Name -t Team1 -t Team2")]
        public void NameAndTeam_ParametersParsing_CheckLogic_Exception(string text)
        {
            Assert.Catch<Exception>(() => new ParametersParser().ParseNameAndTeam(text));
        }

        [TestCase("sdfsdf", 1, "")]
        [TestCase("-1", 1, "")]
        [TestCase("-1.5", 1, "")]
        [TestCase("1,5", 1, "")]
        [TestCase("0", 1, "")]
        [TestCase("5hgffh", 1, "")]
        [TestCase("5 hgffh", 1, "")]
        [TestCase("hg 3", 1, "")]
        [TestCase("45646878964564897465489456489456", 1, "")]
        [TestCase(null, 1, "")]
        [TestCase("", 1, "")]
        [TestCase("       ", 1, "")]
        [TestCase("123 -t Team1", 123, "Team1")]
        [TestCase("123 -to Team1", 1, "")]
        [TestCase("1 23 -t    Team with long   name", 1, "Team with long   name")]
        [TestCase("-t Only team with   long name   ", 1, "Only team with   long name")]
        public void CountAndTeam_Parsing_CheckLogic(string text, int count, string team)
        {
            var result = new ParametersParser().ParseCountAndTeam(text);

            Assert.AreEqual(result.Count, count);
            Assert.AreEqual(result.Team, team);
        }

        [TestCase("123 -t Team1 -t Team2")]
        public void CountAndTeam_ParametersParsing_CheckLogic_Exception(string text)
        {
            Assert.Catch<Exception>(() => new ParametersParser().ParseCountAndTeam(text));
        }
    }
}
