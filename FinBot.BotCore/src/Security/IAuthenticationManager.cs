using System.Threading.Tasks;

namespace FinBot.BotCore.Security {
    public interface IAuthenticationManager {
        
        Task AuthenticateIdentity(IIdentity identity);
        
        Task UnauthenticateIdentity(IIdentity identity);
        
    }
}