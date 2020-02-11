using System;

namespace iRIS.ReviewBot.Bot.Wrappers
{
    public interface IDateTimeWrapper
    {
        DateTime Now
        {
            get;
        }
    }
}