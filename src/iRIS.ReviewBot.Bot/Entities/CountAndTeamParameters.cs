namespace iRIS.ReviewBot.Bot.Entities
{
    public class CountAndTeamParameters
    {
        public int Count { get; }

        public string Team { get; }

        public CountAndTeamParameters(int count, string team)
        {
            Count = count;
            Team = team;
        }
    }
}