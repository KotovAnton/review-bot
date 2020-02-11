using System.Collections.Generic;

namespace iRIS.ReviewBot.Bot
{
    using iRIS.ReviewBot.Bot.Entities;

    public interface IDataProvider
    {
        ChatMember Get(string chatId, string name);

        List<ChatMember> GetAll(string chatId);

        void Add(ChatMember member);

        void Delete(ChatMember member);

        void Update(ChatMember member);
    }
}
