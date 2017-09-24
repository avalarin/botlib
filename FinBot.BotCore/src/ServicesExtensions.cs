using System;
using System.Linq;
using FinBot.BotCore.Commands;
using FinBot.BotCore.Context;
using FinBot.BotCore.Errors;
using FinBot.BotCore.Exceptions;
using FinBot.BotCore.Handlers;
using FinBot.BotCore.Middlewares;
using FinBot.BotCore.ParameterMatching;
using FinBot.BotCore.Rendering;
using FinBot.BotCore.Security;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace FinBot.BotCore {
    public static class ServicesExtensions {
        
        private static IServiceCollection AddMiddlewareHolder(
            this IServiceCollection services, int order,
            Func<MiddlewaresChainBuilder, MiddlewaresChainBuilder> modifier) {

            services.TryAddSingleton(provider => {
                return provider.GetServices<IMiddlewareHolder>()
                    .OrderBy(h => h.Order)
                    .Aggregate(new MiddlewaresChainBuilder(), (chain, holder) => holder.AppendMiddlewares(chain))
                    .Build(new MiddlewaresChainFactory(provider));
            });
            
            return services.AddSingleton<IMiddlewareHolder>(new MiddlewareHolder(order, modifier));
        }
        
        public static IServiceCollection UseExceptionHandler<T>(this IServiceCollection services) where T : class, IExceptionHandler {
            return services.AddSingleton<IExceptionHandler, T>();
        }
        
        public static IServiceCollection UseMessageRenderer<T>(this IServiceCollection services) where T : class, IOutMessageRenderer {
            return services.AddSingleton<IOutMessageRenderer, T>();
        }

        public static IServiceCollection UseHandlers(this IServiceCollection services) {
            return services.AddSingleton<IHandlerFactory, AttributesHandlerFactory>()
                .AddSingleton<IParametersMatcher, DefaultParametersMatcher>()
                .UseExceptionHandler<NoSuchHandlerExceptionHandler>()
                .UseExceptionHandler<UnexpectedExceptionHandler>()
                .AddMiddlewareHolder(1, chain => chain.InsertLast<ContextMiddleware>()
                                                      .InsertLast<RenderingMiddleware>()
                                                      .InsertLast<HandleErrorMiddleware>()
                                                      .InsertLast<ParseCommandMiddleware>()
                                                      .InsertLast<HandlersMiddleware>());
        }

        public static IServiceCollection UseAuthentication(this IServiceCollection services) {
            return services.AddSingleton<IAuthenticationManager, DefaultAuthenticationManager>()
                .AddMiddlewareHolder(2, chain => chain.InsertAfter<AuthenticationMiddleware, HandleErrorMiddleware>());
        }
        
    }
}