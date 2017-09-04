using System;
using FinBot.BotCore.Context;
using FinBot.BotCore.Handlers;
using FinBot.BotCore.Middlewares;
using FinBot.BotCore.ParameterMatching;
using FinBot.BotCore.Telegram.Client;
using FinBot.BotCore.Telegram.Commands;
using FinBot.BotCore.Telegram.Middlewares;
using FinBot.BotCore.Telegram.Polling;
using FinBot.BotCore.Telegram.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FinBot.BotCore {
    public static class ServicesExtensions {

        private static IServiceCollection AddMiddlewareHolder(
            this IServiceCollection services,
            int order,
            Func<MiddlewaresChainBuilder, MiddlewaresChainBuilder> modifier) {

            return services.AddSingleton<IMiddlewareHolder>(new MiddlewareHolder(order, modifier));
        }

        public static IServiceCollection UseTelegramClient(this IServiceCollection services) {
            return services.AddSingleton<ITelegramClient, TelegramClient>()
                .AddSingleton<IAutoPollerConfiguration, AutoPollerAutoConfiguration>()
                .AddSingleton<IClientConfiguration, ClientAutoConfiguration>()
                .AddMiddlewareHolder(1, chain => chain.InsertLast<ContextMiddleware>()
                                                   .InsertLast<RenderringMiddleware>()
                                                   .InsertLast<HandleErrorMiddleware>());
        }
        
        public static IServiceCollection UseHandlers(this IServiceCollection services) {
            return services.AddSingleton<IHandlerFactory, AttributesHandlerFactory>()
                .AddSingleton<IParametersMatcher, DefaultParametersMatcher>()
                .AddSingleton<ICommandParser, DefaultCommandParser>()
                .AddMiddlewareHolder(2, chain => chain.InsertLast<HandlersMiddleware>())
                .AddMiddlewareHolder(3, chain => chain.InsertAfter<ParseCommandMiddleware, HandleErrorMiddleware>());
        }
        
    }
}