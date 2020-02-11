namespace iRIS.ReviewBot.Bot.Wrappers
{
    public class RandomWrapper : IRandomWrapper
    {
        public int Next(int maxValue)
        {
            return RandomProvider.GetThreadRandom().Next(maxValue);
        }
    }
}