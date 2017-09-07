using System.Threading.Tasks;

namespace FinBot.BotCore.Security {
    public interface IIdentityStorage {
        
        Task UpdateAsync(IIdentity identity);

    }
}