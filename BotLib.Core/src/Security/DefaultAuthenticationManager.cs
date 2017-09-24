using System.Threading.Tasks;

namespace BotLib.Core.Security {
    public class DefaultAuthenticationManager : IAuthenticationManager {
        private readonly IIdentiryFactory _identityFactory;
        private readonly IIdentityStorage _identityStorage;

        public DefaultAuthenticationManager(IIdentiryFactory identityFactory, IIdentityStorage identityStorage) {
            _identityFactory = identityFactory;
            _identityStorage = identityStorage;
        }

        public async Task AuthenticateIdentity(IIdentity identity) {
            var newIdentity = await _identityFactory.AuthenticateIdentityAsync(identity);
            await _identityStorage.UpdateAsync(newIdentity);
        }

        public async Task UnauthenticateIdentity(IIdentity identity) {
            var newIdentity = await _identityFactory.UnauthenticateIdentityAsync(identity);
            await _identityStorage.UpdateAsync(newIdentity);
        }
    }
}