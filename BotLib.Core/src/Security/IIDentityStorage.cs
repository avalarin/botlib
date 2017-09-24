using System.Threading.Tasks;

namespace BotLib.Core.Security {
    public interface IIdentityStorage {
        
        Task UpdateAsync(IIdentity identity);

    }
}