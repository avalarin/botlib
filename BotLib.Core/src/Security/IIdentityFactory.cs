﻿using System.Threading.Tasks;

namespace BotLib.Core.Security {
    public interface IIdentiryFactory {
        
        Task<IIdentity> CreateIdentityAsync(IPrincipal principal);

        Task<IIdentity> AuthenticateIdentityAsync(IIdentity identity);
        
        Task<IIdentity> UnauthenticateIdentityAsync(IIdentity identity);

    }
}