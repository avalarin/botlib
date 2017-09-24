using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using BotLib.Core.Middlewares;

namespace BotLib.Core.Security {
    public class AuthenticationMiddleware : IMiddleware {
        private readonly IEnumerable<IAuthenticationHandler> _authenticationHandlers;

        public AuthenticationMiddleware(IEnumerable<IAuthenticationHandler> authenticationHandlers) {
            _authenticationHandlers = authenticationHandlers;
        }

        public async Task<MiddlewareData> InvokeAsync(MiddlewareData data, IMiddlewaresChain chain) {
            var principal = new CombinablePrincipal();

            foreach (var authenticationHandler in _authenticationHandlers) {
                var identity = await authenticationHandler.HandleAuthenticationAsync(data, principal);
                principal = identity.Map(principal.AddIdentity).OrElse(principal);
            }

            var newData = data.UpdateFeatures(f => f.AddExclusive<AuthenticationFeature>(new AuthenticationFeature(principal)));

            return await chain.NextAsync(newData);
        }

        public class CombinablePrincipal : IPrincipal {

            private readonly ImmutableList<IIdentity> _identities;
            
            public IEnumerable<IIdentity> Identities => _identities;
            
            public CombinablePrincipal() {
                _identities = ImmutableList<IIdentity>.Empty;
            }

            private CombinablePrincipal(IEnumerable<IIdentity> identities) {
                _identities = identities.ToImmutableList();
            }

            public CombinablePrincipal AddIdentity(IIdentity identity) {
                return new CombinablePrincipal(_identities.Add(identity));
            }
            
        }
        
    }
}