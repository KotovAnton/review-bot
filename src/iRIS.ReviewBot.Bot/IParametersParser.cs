namespace iRIS.ReviewBot.Bot
{
    using iRIS.ReviewBot.Bot.Entities;

    public interface IParametersParser
    {
        NameAndTeamParameters ParseNameAndTeam(string text);

        CountAndTeamParameters ParseCountAndTeam(string text);
    }
}