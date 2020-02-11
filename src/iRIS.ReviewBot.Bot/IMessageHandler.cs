namespace iRIS.ReviewBot.Bot
{
    using iRIS.ReviewBot.Bot.Entities;

    public interface IMessageHandler
    {
        string Processing(MessageData messageContext);
    }
}