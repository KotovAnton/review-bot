using System;
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
            var expectedResult = $"**Список команд:**{Environment.NewLine}{Environment.NewLine}Добавление ревьювера в список: **add [user name] [-t team]**{Environment.NewLine}Примеры использования:{Environment.NewLine}1)Добавление пользователя с именем Ivan Ivanov{Environment.NewLine}*add Ivan Ivanov*{Environment.NewLine}2)Добавление пользователя с именем Ivan Ivanov из команды Team1{Environment.NewLine}*add Ivan Ivanov -t Team1*{Environment.NewLine}3)Добавление отправителя{Environment.NewLine}*add*{Environment.NewLine}4)Добавление отправителя из команды Team1{Environment.NewLine}*add -t Team1*{Environment.NewLine}{Environment.NewLine}Удаление ревьювера из списка: **delete [user name]**{Environment.NewLine}Примеры использования:{Environment.NewLine}1)Удаление пользователя с именем Ivan Ivanov{Environment.NewLine}*delete Ivan Ivanov*{Environment.NewLine}2)Удаление отправителя{Environment.NewLine}*delete*{Environment.NewLine}{Environment.NewLine}Запросить ревьювера: **get [N] [-t team]**{Environment.NewLine}Примеры использования:{Environment.NewLine}1)Получить одного ревьювера{Environment.NewLine}*get*{Environment.NewLine}2)Получить одного ревьювера из команды Team1{Environment.NewLine}*get -t Team1*{Environment.NewLine}3)Получить список из двух ревьюверов{Environment.NewLine}*get 2*{Environment.NewLine}4)Получить список из двух ревьюверов из команды Team1{Environment.NewLine}*get 2 -t Team1*{Environment.NewLine}{Environment.NewLine}Список ревьюверов: **list**{Environment.NewLine}{Environment.NewLine}**Пожелания/замечания/предложения:**{Environment.NewLine}github: https://github.com/KotovAnton/review-bot/";

            Assert.AreEqual(expectedResult, result);
        }

    }
}
