using System;
using System.Threading.Tasks;
using FinBot.BotCore.Exceptions;
using FinBot.BotCore.Handlers.Filters;
using FinBot.BotCore.Middlewares;
using FinBot.BotCore.Telegram.Features;
using FinBot.BotCore.Telegram.Models;
using FinBot.BotCore.Telegram.Rendering;

namespace FinBot.BotCore.Security {
    public class AuthorizeAttribute : FilterAttribute, IFilter {
        public Task<FilterResult> FilterAsync(FilterAttribute attribute, MiddlewareData data) {
            var principal = data.Features.RequireOne<AuthenticationFeature>().Principal;
            if (!principal.Is​Authenticated()) {
                var message = new MessageContent() {
                    Text = "Unauthorized access"
                };
                var renderer = new ResponseMessageRenderer(message);
                var rendererFeature = new ClientRendererFeature(renderer);;
                var result = FilterResult.BreakExecution(data.UpdateFeatures(f => f.Add<ClientRendererFeature>(rendererFeature)));
                return Task.FromResult(result);
            }
            return Task.FromResult(FilterResult.NextFilter(data));
        }
    }
}