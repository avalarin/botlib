using System;
using System.Collections;
using System.Linq;
using BotLib.Core;
using BotLib.MongoDB;
using BotLib.Telegram;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace BotLib.Example {
    public static class Program {
        
        private static void Main() {
            IServiceProvider serviceProvider = new ServiceCollection()
                .AddLogging(b => b.SetMinimumLevel(LogLevel.Trace).AddNLog())
                .AddSingleton<IConfiguration>(new ConfigurationBuilder()
                    .AddJsonFile("config.json")
                    .AddJsonFile("secret.json", true)
                    .AddEnvironmentVariables("BOT_")
                    .Build())
                .UseTelegramClient()
                .UseMongoDBStorages()
                .UseAuthentication()
                .UseMongoDBAuthentication<ApplicationUser, ApplicationUserFactory>()
                .UseHandlers()
                .BuildServiceProvider();

            BotApplication.Create(serviceProvider).StartAndWait();
        }
        
    }
}