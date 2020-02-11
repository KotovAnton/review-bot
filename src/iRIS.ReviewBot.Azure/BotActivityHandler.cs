using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;

namespace iRIS.ReviewBot.Azure
{
    using iRIS.ReviewBot.Bot;
    using iRIS.ReviewBot.Bot.Entities;

    public class BotActivityHandler : ActivityHandler
    {
        private readonly IMessageHandler _messageHandler;

        public BotActivityHandler(IMessageHandler messageHandler)
        {
            _messageHandler = messageHandler;
        }

        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            string responseText;

            if (IsMessageForAll(turnContext.Activity))
            {
                return;
            }
            else if (!IsGroupChat(turnContext.Activity))
            {
                responseText = "Данный бот работает только в групповых чатах";
            }
            else 
            {
                try
                {
                    var messageContext = GetMessageContext(turnContext.Activity);
                    responseText = _messageHandler.Processing(messageContext);
                }
                catch
                {
                    responseText = "Произошла ошибка обработки команды";
                }
            }

            await turnContext.SendActivityAsync(MessageFactory.Text(responseText), cancellationToken);
        }

        private MessageData GetMessageContext(IMessageActivity messageActivity)
        {
            return new MessageData(messageActivity.From.Name.Trim(), messageActivity.Conversation.Id, messageActivity.Recipient.Name, messageActivity.Text.Trim());
        }

        private bool IsGroupChat(IMessageActivity messageActivity)
        {
            return messageActivity.Conversation.IsGroup == true;
        }

        private bool IsMessageForAll(IMessageActivity messageActivity)
        {
            string[] versionsOfAll = new[] { "all", "все" };

            return messageActivity.Text
                .Trim()
                .Split(' ')
                .Any(w => versionsOfAll.Contains(w));
        }
    }
}
