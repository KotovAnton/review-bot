namespace iRIS.ReviewBot.Bot.Commands
{
    using iRIS.ReviewBot.Bot.Entities;

    public interface ICommand
    {
        CommandType Type { get; }

        string Execute(CommandContext context);
    }
}