﻿using System.Threading.Tasks;
using FinBot.BotCore.Middlewares;

namespace FinBot.BotCore.Handlers {
    public static class HandlerResultCreators {

        public static IHandlerResult Text(string text) {
            return new HandlerResultBuilder()
                .Text(text)
                .Create();
        }

        public static IHandlerResult Join(this IHandlerResult a, IHandlerResult b) {
            return new JoiningHandlerResult(a, b);
        }

        private class JoiningHandlerResult : IHandlerResult {
            private readonly IHandlerResult _a;
            private readonly IHandlerResult _b;

            public JoiningHandlerResult(IHandlerResult a, IHandlerResult b) {
                _a = a;
                _b = b;
            }

            public async Task<MiddlewareData> RenderAsync(MiddlewareData data) {
                var aData = await _a.RenderAsync(data);
                return await _b.RenderAsync(aData);
            }
        }
        
    }
}