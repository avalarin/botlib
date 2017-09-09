using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FinBot.BotCore.Exceptions;
using FinBot.BotCore.Handlers.Filters;
using FinBot.BotCore.Middlewares;
using FinBot.BotCore.ParameterMatching;
using FinBot.BotCore.Utils;

namespace FinBot.BotCore.Handlers {
    public class AttributesHandlerFactory : IHandlerFactory {
        private readonly IServiceProvider _serviceProvider;
        private readonly IParametersMatcher _parametersMatcher;
        private readonly List<HandlerDescriptor> _descriptors;

        public AttributesHandlerFactory(IServiceProvider serviceProvider, IParametersMatcher parametersMatcher) {
            _serviceProvider = serviceProvider;
            _parametersMatcher = parametersMatcher;
            _descriptors = Assembly.GetEntryAssembly()
                .GetTypes()
                .Where(t => t.Name.EndsWith("Controller"))
                .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.Public).Select(method => new {type, method}))
                .SelectMany(tuple => tuple.method.GetCustomAttributes<HandlerAttribute>().Select(attr => new {tuple.type, tuple.method, attr}))
                .Select(tuple => HandlerDescriptor.Create(tuple.method, tuple.attr))
                .ToList();
        } 
        
        public IEnumerable<IHandler> CreateHandlers(MiddlewareData middlewareData) {
            return _descriptors
                .Select(d => d.Match(middlewareData))
                .Where(m => m.Successful)
                .Select(m => new Handler(_serviceProvider, _parametersMatcher, m.Descriptor.Method));
        }

        private class Handler : IHandler {
            private readonly IServiceProvider _serviceProvider;
            private readonly IParametersMatcher _parametersMatcher;
            private readonly MethodInfo _method;

            public Handler(IServiceProvider serviceProvider, IParametersMatcher parametersMatcher, MethodInfo method) {
                _serviceProvider = serviceProvider;
                _parametersMatcher = parametersMatcher;
                _method = method;
            }

            public async Task<IHandlerResult> ExecuteAsync(MiddlewareData middlewareData) {
                var filters = _method.GetCustomAttributes<FilterAttribute>();
                var filterResult = await FilterUtils.ExecuteFilters(filters, _serviceProvider, middlewareData);
                if (filterResult.Action == FilterAction.SkipHandler) {
                    return null;
                }

                if (filterResult.Action == FilterAction.BreakExecution) {
                    return filterResult.MiddlewareData
                        .Map(HandlerResultCreators.UpdateMiddlewareData)
                        .OrElseGet(HandlerResultCreators.Empty);
                }

                var parameters = _parametersMatcher.MatchParameters(filterResult.MiddlewareData.Value, _method.GetParameters());
                if (!parameters.IsPresent) {
                    return null;
                }
                
                var instance = _serviceProvider.GetInstance(_method.DeclaringType);
                var result = _method.Invoke(instance, parameters.Value.Select(v => v.Value).ToArray());
                if (result is Task<IHandlerResult> taskHandlerResult) {
                    return await taskHandlerResult;
                }
                if (result is Task<IEnumerable<IHandlerResult>> taskHandlerResults) {
                    return HandlerResultCreators.JoinAll(await taskHandlerResults);
                }
                if (result is IHandlerResult handlerResult) {
                    return handlerResult;
                }
                if (result is IEnumerable<IHandlerResult> handlerResults) {
                    return HandlerResultCreators.JoinAll(handlerResults);
                }
                throw new InvalidOperationException("Cannot create result from type " + _method.ReturnType);
            }
        }
         
    }
}