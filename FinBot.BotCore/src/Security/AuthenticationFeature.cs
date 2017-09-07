using System.Collections.Generic;
using FinBot.BotCore.Middlewares;
using FinBot.BotCore.ParameterMatching;

namespace FinBot.BotCore.Security {
    public class AuthenticationFeature : IFeature, IParameterValuesSource {
        
        public IPrincipal Principal { get; }

        public AuthenticationFeature(IPrincipal principal) {
            Principal = principal;
        }

        public IEnumerable<ParameterValue> GetValues() {
            yield return new ParameterValue("principal", Principal);
            foreach (var identity in Principal.Identities) {
                yield return new ParameterValue("identity", identity);
            }
        }
    }
}