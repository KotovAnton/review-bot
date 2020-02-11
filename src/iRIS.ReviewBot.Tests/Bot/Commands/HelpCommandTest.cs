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

            Assert.AreEqual(helpCommand.Type, CommandType.Help);
        }

        [Test]
        public void HelpCommand_CheckLogic()
        {
            var helpCommand = new HelpCommand();
            var context = new CommandContext(null, null, null);

            var result = helpCommand.Execute(context);
            var text = @"**Список команд:**

Добавление ревьюера в список: **add [user name] [-t team]**
Примеры использования:
1)Добавление пользователя с именем Ivan Ivanov
*add Ivan Ivanov*
2)Добавление пользователя с именем Ivan Ivanov из команды Team1
*add Ivan Ivanov -t Team1*
3)Добавление отправителя
*add*
4)Добавление отправителя из команды Team1
*add -t Team1*

Удаление ревьюера из списка: **delete [user name]**
Примеры использования:
1)Удаление пользователя с именем Ivan Ivanov
*delete Ivan Ivanov*
2)Удаление отправителя
*delete*

Запросить ревьюера: **get [N] [-t team]**
Примеры использования:
1)Получить одного ревьюера
*get*
2)Получить одного ревьюера из команды Team1
*get -t Team1*
3)Получить список из двух ревьюеров
*get 2*
4)Получить список из двух ревьюеров из команды Team1
*get 2 -t Team1*

Список ревьюеров: **list**";

            Assert.AreEqual(text, result);
        }

    }
}
