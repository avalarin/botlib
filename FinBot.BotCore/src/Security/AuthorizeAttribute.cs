using System.Threading.Tasks;
using FinBot.BotCore.Exceptions;
using FinBot.BotCore.Handlers.Filters;
using FinBot.BotCore.Middlewares;

namespace FinBot.BotCore.Security {
    public class AuthorizeAttribute : FilterAttribute, IFilter {
        public Task<FilterResult> FilterAsync(FilterAttribute attribute, MiddlewareData data) {
            var principal = data.Features.RequireOne<AuthenticationFeature>().Principal;
            if (!principal.Is​Authenticated()) {
                throw new UnauthorizedException();
            }
            return Task.FromResult(FilterResult.CreateSuccessful(data));
        }
    }
}