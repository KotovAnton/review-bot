using System;
using Dapper.Contrib.Extensions;

namespace iRIS.ReviewBot.Bot.Entities
{
    public class ChatMember
    {
        [Key]
        public int Id { get; set; }

        public string ChatId { get; set; }

        public string Name { get; set; }

        public string Team { get; set; }

        public DateTime LastReview { get; set; }
    }
}