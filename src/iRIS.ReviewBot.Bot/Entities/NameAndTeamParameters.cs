namespace iRIS.ReviewBot.Bot.Entities
{
    public class NameAndTeamParameters
    {
        public string Name { get; }

        public string Team { get; }

        public NameAndTeamParameters(string name, string team)
        {
            Name = name;
            Team = team;
        }
    }
}