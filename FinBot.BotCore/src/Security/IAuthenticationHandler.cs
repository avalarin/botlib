using System.Threading.Tasks;
using FinBot.BotCore.Middlewares;
using FinBot.BotCore.Utils;

namespace FinBot.BotCore.Security {
    public interface IAuthenticationHandler {

        Task<Maybe<IIdentity>> HandleAuthenticationAsync(MiddlewareData data, IPrincipal principal);

    }
}