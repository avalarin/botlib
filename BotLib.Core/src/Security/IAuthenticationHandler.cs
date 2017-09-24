using System.Threading.Tasks;
using BotLib.Core.Middlewares;
using BotLib.Core.Utils;

namespace BotLib.Core.Security {
    public interface IAuthenticationHandler {

        Task<Maybe<IIdentity>> HandleAuthenticationAsync(MiddlewareData data, IPrincipal principal);

    }
}