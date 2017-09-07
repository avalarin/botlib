using System.Linq;
using FinBot.BotCore.Utils;

namespace FinBot.BotCore.Security {
    public static class PrincipalExtensions {
        
        public static Maybe<T> GetIdentity<T>(this IPrincipal principal) where T : IIdentity {
            return principal.Identities.OfType<T>().SingleOrDefault().Nullable();
        }

        public static bool Is​Authenticated(this IPrincipal principal) {
            return principal.Identities.Any(i => i.Is​Authenticated);
        }
        
    }
}