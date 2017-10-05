using System;
using System.IO;
using BotLib.Core;
using BotLib.Core.Utils;
using BotLib.MongoDB;
using BotLib.Telegram;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Config;
using NLog.Extensions.Logging;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace BotLib.Example {
    public static class Program {
        
        private static void Main() {
            IServiceProvider serviceProvider = new ServiceCollection()
                .UseLogging()
                .UseConfiguration()
                .UseTelegramClient()
                .UseMongoDBStorages()
                .UseAuthentication()
                .UseMongoDBAuthentication<ApplicationUser, ApplicationUserFactory>()
                .UseHandlers()
                .BuildServiceProvider();
            
            BotApplication.Create(serviceProvider).StartAndWait();
        }

        private static IServiceCollection UseConfiguration(this IServiceCollection services) {
            var configBaseDirectory = "";
            
            Environment.GetEnvironmentVariable("BOT_CONFIG_DIR").Nullable()
                .Filter(str => !string.IsNullOrWhiteSpace(str))
                .IfPresent(path => {
                    configBaseDirectory = Path.Combine(Directory.GetCurrentDirectory(), path);
                    Console.WriteLine($"Using the application configuration from {Path.Combine(configBaseDirectory, "config.json")} and {Path.Combine(configBaseDirectory, "secret.json")}");
                    
                });
            
            services.AddSingleton<IConfiguration>(new ConfigurationBuilder()
                .AddJsonFile(Path.Combine(configBaseDirectory, "config.json"))
                .AddJsonFile(Path.Combine(configBaseDirectory, "secret.json"), true)
                .AddEnvironmentVariables("BOT_")
                .Build()
            );

            return services;
        }
        
        private static IServiceCollection UseLogging(this IServiceCollection services) {
            services.AddLogging(b => b
                .SetMinimumLevel(LogLevel.Trace)
                .AddNLog()
            );

            Environment.GetEnvironmentVariable("BOT_CONFIG_DIR").Nullable()
                .Filter(str => !string.IsNullOrWhiteSpace(str))
                .IfPresent(path => {
                    var fullPath = Path.Combine(path, "nlog.config");
                    Console.WriteLine($"Using the logging configuration from {fullPath}");
                    LogManager.Configuration = (LoggingConfiguration) new XmlLoggingConfiguration(fullPath, false);
                });
            
            return services;
        }
        
    }
}