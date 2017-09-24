using System;
using System.Linq;
using BotLib.Core.Commands;
using BotLib.Core.Context;
using BotLib.Core.Errors;
using BotLib.Core.Handlers;
using BotLib.Core.Middlewares;
using BotLib.Core.ParameterMatching;
using BotLib.Core.Rendering;
using BotLib.Core.Security;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace BotLib.Core {
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