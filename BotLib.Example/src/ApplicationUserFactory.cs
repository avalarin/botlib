using System;
using System.Threading.Tasks;
using BotLib.Core.Security;
using BotLib.Telegram.Security;

namespace BotLib.Example {
    public class ApplicationUserFactory : IIdentiryFactory {

        public Task<IIdentity> CreateIdentityAsync(IPrincipal principal) {
            var telegramIdentity = principal.GetIdentity<TelegramIdentity>()
                .OrElseThrow(() => new InvalidOperationException("TelegramIdentity is required"));
            
            IIdentity user = ApplicationUser.Create(telegramIdentity);
            return Task.FromResult(user);
        }

        public Task<IIdentity> AuthenticateIdentityAsync(IIdentity identity) {
            IIdentity newIdentity = ((ApplicationUser) identity).Authenticated();
            return Task.FromResult(newIdentity);
        }

        public Task<IIdentity> UnauthenticateIdentityAsync(IIdentity identity) {
            IIdentity newIdentity = ((ApplicationUser) identity).Unauthenticated();
            return Task.FromResult(newIdentity);
        }

    }
}