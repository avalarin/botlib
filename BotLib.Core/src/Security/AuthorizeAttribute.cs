using System.Threading.Tasks;
using BotLib.Core.Handlers.Filters;
using BotLib.Core.Middlewares;
using BotLib.Core.Rendering;

namespace BotLib.Core.Security {
    public class AuthorizeAttribute : FilterAttribute, IFilter {
        public Task<FilterResult> FilterAsync(FilterAttribute attribute, MiddlewareData data) {
            var principal = data.Features.RequireOne<AuthenticationFeature>().Principal;
            if (!principal.Is​Authenticated()) {
                var message = new BaseOutMessage() {
                    Text = "Unauthorized access"
                };
                var result = FilterResult.BreakExecution(data.AddRenderMessageFeature(message));
                return Task.FromResult(result);
            }
            return Task.FromResult(FilterResult.NextFilter(data));
        }
    }
}