namespace iRIS.ReviewBot.Bot.Entities
{
    public class CommandContext
    {
        public string SenderName { get; private set; }

        public string ChatId { get; private set; }

        public string Parameters { get; private set; }

        public CommandContext(string senderName, string chatId, string parameters)
        {
            SenderName = senderName;
            ChatId = chatId;
            Parameters = parameters;
        }
    }
}