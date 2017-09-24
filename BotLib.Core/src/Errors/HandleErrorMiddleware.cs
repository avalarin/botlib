using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using BotLib.Core.Middlewares;

namespace BotLib.Core.Errors {
    public class HandleErrorMiddleware : IMiddleware {
        private readonly ConcurrentDictionary<Type, IExceptionHandler> _handlersCache;
        
        public HandleErrorMiddleware(
            IEnumerable<IExceptionHandler> handlers
        ) { var types = handlers
                .SelectMany(r => GetHandlingExceptionTypes(r.GetType()).Select(t => new KeyValuePair<Type, IExceptionHandler>(t, r)));
            _handlersCache = new ConcurrentDictionary<Type, IExceptionHandler>(types);
        }

        public async Task<MiddlewareData> InvokeAsync(MiddlewareData data, IMiddlewaresChain chain) {
            try {
                return await chain.NextAsync(data);
            }
            catch (Exception e) {
                return GetHandler(e.GetType()).HandleException(data, e);
            }
        }
        
        private IExceptionHandler GetHandler(Type exceptionType) {
            return _handlersCache.GetOrAdd(exceptionType, t => {
                if (exceptionType == typeof(Exception)) return null;
                var baseType = exceptionType.GetTypeInfo().BaseType;
                return GetHandler(baseType);
            }) ?? throw new InvalidOperationException("Cannot find handler for exception of type " + exceptionType.Name);
        }
        
        private IEnumerable<Type> GetHandlingExceptionTypes(Type handlerType) {
            return handlerType.GetInterfaces()
                .Where(iface => iface.IsConstructedGenericType && iface.GetGenericTypeDefinition() == typeof(IExceptionHandler<>))
                .Select(iface => iface.GenericTypeArguments[0]);
        }
    }
}