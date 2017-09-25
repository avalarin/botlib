using System;
using System.Collections;
using System.Linq;
using BotLib.Core;
using BotLib.MongoDB;
using BotLib.Telegram;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BotLib.Example {
    public static class Program {
        
        private static void Main() {
            Console.WriteLine("Starting");
            foreach (var v in Environment.GetEnvironmentVariables().Cast<DictionaryEntry>()) {
                Console.WriteLine($"{v.Key}={v.Value}");
            }
            
            IServiceProvider serviceProvider = new ServiceCollection()
                .AddSingleton<ILoggerFactory>(new LoggerFactory().AddConsole())
                .AddLogging()
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

            BotApplication.Create(serviceProvider).StartAndLock();
        }
        
    }
}