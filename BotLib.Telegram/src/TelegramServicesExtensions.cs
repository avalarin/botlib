using BotLib.Core;
using BotLib.Core.Commands;
using BotLib.Core.Context;
using BotLib.Core.Security;
using BotLib.Telegram.Client;
using BotLib.Telegram.Commands;
using BotLib.Telegram.Context;
using BotLib.Telegram.Polling;
using BotLib.Telegram.Rendering;
using BotLib.Telegram.Security;
using Microsoft.Extensions.DependencyInjection;

namespace BotLib.Telegram {
    public static class TelegramServicesExtensions {
        
        public static IServiceCollection UseTelegramClient(this IServiceCollection services) {
            return services.AddSingleton<ITelegramClient, TelegramClient>()
                .AddSingleton<AutoPoller>()
                .AddSingleton<IAutoPollerConfiguration, AutoPollerAutoConfiguration>()
                .AddSingleton<IClientConfiguration, ClientAutoConfiguration>()
                .AddSingleton<ICommandParser, TelegramCommandParser>()
                .AddSingleton<IAuthenticationHandler, TelegramAuthenticationHandler>()
                .AddSingleton<IContextManager, TelegramContextManager>()
                .UseMessageRenderer<TelegramOutMessageRenderer>();
        }
        
    }
}