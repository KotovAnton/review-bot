using System.Text;

namespace iRIS.ReviewBot.Bot.Commands
{
    using iRIS.ReviewBot.Bot.Entities;

    public class HelpCommand : ICommand
    {
        public CommandType Type { get; }

        public HelpCommand()
        {
            Type = CommandType.Help;
        }

        public string Execute(CommandContext context)
        {
            var sb = new StringBuilder(500);
            sb.AppendLine("**Список команд:**");

            sb.AppendLine();
            sb.AppendLine("Добавление ревьюера в список: **add [user name] [-t team]**");
            sb.AppendLine("Примеры использования:");
            sb.AppendLine("1)Добавление пользователя с именем Ivan Ivanov");
            sb.AppendLine("*add Ivan Ivanov*");
            sb.AppendLine("2)Добавление пользователя с именем Ivan Ivanov из команды Team1");
            sb.AppendLine("*add Ivan Ivanov -t Team1*");
            sb.AppendLine("3)Добавление отправителя");
            sb.AppendLine("*add*");
            sb.AppendLine("4)Добавление отправителя из команды Team1");
            sb.AppendLine("*add -t Team1*");

            sb.AppendLine();
            sb.AppendLine("Удаление ревьюера из списка: **delete [user name]**");
            sb.AppendLine("Примеры использования:");
            sb.AppendLine("1)Удаление пользователя с именем Ivan Ivanov");
            sb.AppendLine("*delete Ivan Ivanov*");
            sb.AppendLine("2)Удаление отправителя");
            sb.AppendLine("*delete*");

            sb.AppendLine();
            sb.AppendLine("Запросить ревьюера: **get [N] [-t team]**");
            sb.AppendLine("Примеры использования:");
            sb.AppendLine("1)Получить одного ревьюера");
            sb.AppendLine("*get*");
            sb.AppendLine("2)Получить одного ревьюера из команды Team1");
            sb.AppendLine("*get -t Team1*");
            sb.AppendLine("3)Получить список из двух ревьюеров");
            sb.AppendLine("*get 2*");
            sb.AppendLine("4)Получить список из двух ревьюеров из команды Team1");
            sb.AppendLine("*get 2 -t Team1*");

            sb.AppendLine();
            sb.Append("Список ревьюеров: **list**");

            return sb.ToString();
        }
    }
}