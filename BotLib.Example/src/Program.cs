using System;
using BotLib.Core;
using BotLib.MongoDB;
using BotLib.Telegram;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BotLib.Example {
    public static class Program {
        
        private static void Main() {
            IServiceProvider serviceProvider = new ServiceCollection()
                .AddSingleton<ILoggerFactory>(new LoggerFactory().AddConsole())
                .AddLogging()
                .AddSingleton<IConfiguration>(new ConfigurationBuilder()
                    .AddJsonFile("config.json")
                    .AddJsonFile("secret.json", true)
                    .Build())
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