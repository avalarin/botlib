using System.Threading.Tasks;
using BotLib.Core.Middlewares;
using BotLib.Core.Security;
using BotLib.Core.Utils;
using BotLib.Telegram.Features;

namespace BotLib.Telegram.Security {
    public class TelegramAuthenticationHandler : IAuthenticationHandler {
        public Task<Maybe<IIdentity>> HandleAuthenticationAsync(MiddlewareData data, IPrincipal principal) {
            var message = data.Features.RequireOne<UpdateInfoFeature>().GetAnyMessage();
            if (message == null) {
                return Task.FromResult(Maybe<IIdentity>.Empty());
            }
            var userId = message.User?.Id ?? message.Chat.Id;
            var userName = message.User?.UserName ?? message.Chat.UserName;
            var identity = new TelegramIdentity(userId.ToString(), userName);
            return Task.FromResult(Maybe<IIdentity>.Of(identity));
        }
    }
}