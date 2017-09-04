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
        
        public Task<IHandler> CreateHandlerAsync(MiddlewareData middlewareData) {
            IHandler handler = _descriptors
                .Select(d => d.Match(middlewareData, _serviceProvider))
                .FirstOrDefault(m => m.Successful)
                .Nullable()
                .Map(m => new Handler(_serviceProvider, m.Descriptor.Method, m.Values))
                .OrElseThrow(() => new NoSuchHandlerException());
            return Task.FromResult(handler);
        }

        public class Handler : IHandler {
            private readonly IServiceProvider _serviceProvider;
            private readonly MethodInfo _method;
            private readonly ParameterValue[] _values;

            public Handler(IServiceProvider serviceProvider, MethodInfo method, ParameterValue[] values) {
                _serviceProvider = serviceProvider;
                _method = method;
                _values = values;
            }

            public Task<IHandlerResult> ExecuteAsync(MiddlewareData middlewareData) {
                var instance = _serviceProvider.GetInstance(_method.DeclaringType);
                var result = _method.Invoke(instance, _values.Select(v => v.Value).ToArray());
                if (result is Task<IHandlerResult> taskHandlerResult) {
                    return taskHandlerResult;
                }
                if (result is IHandlerResult handlerResult) {
                    return Task.FromResult(handlerResult);
                }
                throw new InvalidOperationException("Cannot create result from type " + _method.ReturnType);
            }
        }
         
    }
}