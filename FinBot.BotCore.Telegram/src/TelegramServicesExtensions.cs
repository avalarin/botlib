using FinBot.BotCore.Commands;
using FinBot.BotCore.Context;
using FinBot.BotCore.Rendering;
using FinBot.BotCore.Security;
using FinBot.BotCore.Telegram.Client;
using FinBot.BotCore.Telegram.Commands;
using FinBot.BotCore.Telegram.Context;
using FinBot.BotCore.Telegram.Polling;
using FinBot.BotCore.Telegram.Rendering;
using FinBot.BotCore.Telegram.Security;
using Microsoft.Extensions.DependencyInjection;

namespace FinBot.BotCore.Telegram {
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