using System;

namespace iRIS.ReviewBot.Bot.Wrappers
{
    public class DateTimeWrapper : IDateTimeWrapper
    {
        public DateTime Now
        {
            get => DateTime.Now;
        }
    }
}