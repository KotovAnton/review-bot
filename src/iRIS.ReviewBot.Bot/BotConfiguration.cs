using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace iRIS.ReviewBot.Bot
{
    using Commands;
    using Wrappers;

    public static class BotConfiguration
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddScoped<IMessageHandler, MessageHandler>();
            services.AddScoped<IParametersParser, ParametersParser>();
            services.AddScoped<IDataProvider, SqlDbProvider>((serviceProvider) => new SqlDbProvider(connectionString));
            services.AddScoped<IRandomWrapper, RandomWrapper>();
            services.AddScoped<IDateTimeWrapper, DateTimeWrapper>();
            services.AddScoped((serviceProvider) =>
            {
                return new List<ICommand>
                {
                    new AddCommand(serviceProvider.GetService<IDataProvider>(), serviceProvider.GetService<IDateTimeWrapper>(), serviceProvider.GetService<IParametersParser>()),
                    new DeleteCommand(serviceProvider.GetService<IDataProvider>()),
                    new GetCommand(serviceProvider.GetService<IDataProvider>(), serviceProvider.GetService<IRandomWrapper>(), serviceProvider.GetService<IDateTimeWrapper>(), serviceProvider.GetService<IParametersParser>()),
                    new HelpCommand(),
                    new ListCommand(serviceProvider.GetService<IDataProvider>()),
                    new UnknownCommand()
                };
            });
        }
    }
}
