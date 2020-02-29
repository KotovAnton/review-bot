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
            sb.AppendLine("Добавление ревьювера в список: **add [user name] [-t team]**");
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
            sb.AppendLine("Удаление ревьювера из списка: **delete [user name]**");
            sb.AppendLine("Примеры использования:");
            sb.AppendLine("1)Удаление пользователя с именем Ivan Ivanov");
            sb.AppendLine("*delete Ivan Ivanov*");
            sb.AppendLine("2)Удаление отправителя");
            sb.AppendLine("*delete*");

            sb.AppendLine();
            sb.AppendLine("Запросить ревьювера: **get [N] [-t team]**");
            sb.AppendLine("Примеры использования:");
            sb.AppendLine("1)Получить одного ревьювера");
            sb.AppendLine("*get*");
            sb.AppendLine("2)Получить одного ревьювера из команды Team1");
            sb.AppendLine("*get -t Team1*");
            sb.AppendLine("3)Получить список из двух ревьюверов");
            sb.AppendLine("*get 2*");
            sb.AppendLine("4)Получить список из двух ревьюверов из команды Team1");
            sb.AppendLine("*get 2 -t Team1*");

            sb.AppendLine();
            sb.AppendLine("Список ревьюверов: **list**");

            sb.AppendLine();
            sb.AppendLine("**Пожелания/замечания/предложения:**");
            sb.Append("github: https://github.com/KotovAnton/review-bot/");

            return sb.ToString();
        }
    }
}