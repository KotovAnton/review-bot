using NUnit.Framework;

namespace iRIS.ReviewBot.Tests.Bot.Commands
{
    using iRIS.ReviewBot.Bot.Entities;
    using iRIS.ReviewBot.Bot.Commands;

    public class HelpCommandTest
    {
        [Test]
        public void HelpCommand_CheckCommandType()
        {
            var helpCommand = new HelpCommand();

            Assert.AreEqual(CommandType.Help, helpCommand.Type);
        }

        [Test]
        public void HelpCommand_CheckLogic()
        {
            var helpCommand = new HelpCommand();
            var context = new CommandContext(null, null, null);

            var result = helpCommand.Execute(context);
            var expectedResult = "**Список команд:**\r\n\r\nДобавление ревьюера в список: **add [user name] [-t team]**\r\nПримеры использования:\r\n1)Добавление пользователя с именем Ivan Ivanov\r\n*add Ivan Ivanov*\r\n2)Добавление пользователя с именем Ivan Ivanov из команды Team1\r\n*add Ivan Ivanov -t Team1*\r\n3)Добавление отправителя\r\n*add*\r\n4)Добавление отправителя из команды Team1\r\n*add -t Team1*\r\n\r\nУдаление ревьюера из списка: **delete [user name]**\r\nПримеры использования:\r\n1)Удаление пользователя с именем Ivan Ivanov\r\n*delete Ivan Ivanov*\r\n2)Удаление отправителя\r\n*delete*\r\n\r\nЗапросить ревьюера: **get [N] [-t team]**\r\nПримеры использования:\r\n1)Получить одного ревьюера\r\n*get*\r\n2)Получить одного ревьюера из команды Team1\r\n*get -t Team1*\r\n3)Получить список из двух ревьюеров\r\n*get 2*\r\n4)Получить список из двух ревьюеров из команды Team1\r\n*get 2 -t Team1*\r\n\r\nСписок ревьюеров: **list**";

            Assert.AreEqual(expectedResult, result);
        }

    }
}
