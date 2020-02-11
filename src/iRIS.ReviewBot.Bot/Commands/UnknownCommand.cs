using System.Text;

namespace iRIS.ReviewBot.Bot.Commands
{
    using iRIS.ReviewBot.Bot.Entities;

    public class UnknownCommand : ICommand
    {
        public CommandType Type { get; }


        public UnknownCommand()
        {
            Type = CommandType.Unknown;
        }

        public string Execute(CommandContext context)
        {
            var sb = new StringBuilder();

            sb.AppendLine("**Неизвестная команда...**");
            sb.Append("Список доступных команд можно просмотреть отправив **help**");

            return sb.ToString();
        }
    }
}
