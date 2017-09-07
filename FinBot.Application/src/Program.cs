using FinBot.BotCore;
using FinBot.BotCore.MongoDB;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Bson.Serialization;

namespace FinBot.Application {
    public static class Program {
        
        private static void Main() {
            var serviceProvider = new ServiceCollection()
                .AddSingleton<ILoggerFactory>(new LoggerFactory().AddConsole())
                .AddSingleton<IConfiguration>(new ConfigurationBuilder().AddJsonFile("config.json").Build())
                .UseTelegramClient()
                .UseMongoDBStorages()
                .UseAuthentication()
                .UseMongoDBAuthentication<ApplicationUser, ApplicationUserFactory>()
                .UseHandlers()
                .BuildServiceProvider();

            BotApplication.Create(serviceProvider).StartAndLock();
        }
        
    }
}