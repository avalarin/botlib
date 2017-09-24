using System.Threading.Tasks;

namespace BotLib.Core.Security {
    public interface IAuthenticationManager {
        
        Task AuthenticateIdentity(IIdentity identity);
        
        Task UnauthenticateIdentity(IIdentity identity);
        
    }
}