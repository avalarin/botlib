using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FinBot.BotCore.Middlewares;

namespace FinBot.BotCore.Rendering {
    public class RenderingMiddleware : IMiddleware {
        private readonly ConcurrentDictionary<Type, IOutMessageRenderer> _renderersCache;
        
        public RenderingMiddleware(IEnumerable<IOutMessageRenderer> renderers) {
            var types = renderers
                .SelectMany(r => GetRenderingMessageTypes(r.GetType()).Select(t => new KeyValuePair<Type, IOutMessageRenderer>(t, r)));
            _renderersCache = new ConcurrentDictionary<Type, IOutMessageRenderer>(types);
        }

        public async Task<MiddlewareData> InvokeAsync(MiddlewareData data, IMiddlewaresChain chain) {
            var resultData = await chain.NextAsync(data);
            foreach (var message in resultData.Features.GetAll<RenderMessageFeature>().Select(f => f.Message)) {
                var renderrer = GetRenderer(message.GetType());
                resultData = await renderrer.RenderAsync(resultData, message);
            }
            return resultData;
        }

        private IOutMessageRenderer GetRenderer(Type messageType) {
            return _renderersCache.GetOrAdd(messageType, t => {
                if (messageType == typeof(BaseOutMessage)) return null;
                var baseType = messageType.GetTypeInfo().BaseType;
                return GetRenderer(baseType);
            }) ?? throw new InvalidOperationException("Cannot find renderer for message of type " + messageType.Name);
        }

        private static IEnumerable<Type> GetRenderingMessageTypes(Type rendererType) {
            return rendererType.GetInterfaces()
                .Where(iface => iface.IsConstructedGenericType && iface.GetGenericTypeDefinition() == typeof(IOutMessageRenderer<>))
                .Select(iface => iface.GenericTypeArguments[0]);
        }
    }
}