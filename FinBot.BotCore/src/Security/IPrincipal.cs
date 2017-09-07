using System.Collections.Generic;

namespace FinBot.BotCore.Security {
    public interface IPrincipal {

        IEnumerable<IIdentity> Identities { get; }

    }
}