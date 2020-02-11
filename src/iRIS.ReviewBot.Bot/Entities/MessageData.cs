namespace iRIS.ReviewBot.Bot.Entities
{
    public class MessageData
    {
        public string SenderName { get; }

        public string ChatId { get; }

        public string RecipientName { get; }

        public string MessageText { get; }

        public MessageData(string senderName, string chatId, string recipientName, string messageText)
        {
            SenderName = senderName;
            ChatId = chatId;
            RecipientName = recipientName;
            MessageText = messageText;
        }
    }
}