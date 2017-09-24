using System.Collections.Generic;

namespace BotLib.Core.Security {
    public interface IPrincipal {

        IEnumerable<IIdentity> Identities { get; }

    }
}