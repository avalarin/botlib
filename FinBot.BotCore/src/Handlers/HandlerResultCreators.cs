using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FinBot.BotCore.Context;
using FinBot.BotCore.Middlewares;

namespace FinBot.BotCore.Handlers {
    public static class HandlerResultCreators {

        public static IHandlerResult Join(this IHandlerResult a, IHandlerResult b) {
            return new JoiningHandlerResult(a, b);
        }
        
        public static IHandlerResult JoinAll(IEnumerable<IHandlerResult> results) {
            return new JoiningHandlerResult(results);
        }
        
        public static IHandlerResult UpdateMiddlewareData(MiddlewareData middlewareData) {
            return new UpdateMiddlewareDataHandlerResult(middlewareData);
        }
        
        public static IHandlerResult Empty() {
            return new EmptyMiddlewareDataHandlerResult();
        }

        public static IHandlerResult PutToMessageContext(string key, object value) {
            return new UpdateMiddlewareDataHandlerResult(
                data => data.UpdateFeatures(
                    f => f.ReplaceExclusive<MessageContextFeature>(
                        context => context.Put(key, value)
                    )
                )
            );
        }
        
        public static IHandlerResult PutToChatContext(string key, object value) {
            return new UpdateMiddlewareDataHandlerResult(
                data => data.UpdateFeatures(
                    f => f.ReplaceExclusive<ChatContextFeature>(
                        context => context.Put(key, value)
                    )
                )
            );
        }

        private class EmptyMiddlewareDataHandlerResult : IHandlerResult {
            public Task<MiddlewareData> RenderAsync(MiddlewareData data) {
                return Task.FromResult(data);
            }
        }

        private class UpdateMiddlewareDataHandlerResult : IHandlerResult {
            private readonly MiddlewareData _middlewareData;
            private readonly Func<MiddlewareData, MiddlewareData> _func;

            public UpdateMiddlewareDataHandlerResult(MiddlewareData middlewareData) {
                _middlewareData = middlewareData;
                _func = null;
            }

            public UpdateMiddlewareDataHandlerResult(Func<MiddlewareData, MiddlewareData> func) {
                _middlewareData = null;
                _func = func;
            }

            public Task<MiddlewareData> RenderAsync(MiddlewareData data) {
                if (_func != null) {
                    return Task.FromResult(_func(data));
                }
                return Task.FromResult(_middlewareData);
            }
        } 
        
        
        private class JoiningHandlerResult : IHandlerResult {
            private readonly IEnumerable<IHandlerResult> _results;

            public JoiningHandlerResult(params IHandlerResult[] results) {
                _results = results;
            }
            
            public JoiningHandlerResult(IEnumerable<IHandlerResult> results) {
                _results = results;
            }

            public async Task<MiddlewareData> RenderAsync(MiddlewareData data) {
                var lastData = data;
                foreach (var result in _results) {
                    lastData = await result.RenderAsync(lastData);
                }
                return lastData;
            }
        }
    }
}