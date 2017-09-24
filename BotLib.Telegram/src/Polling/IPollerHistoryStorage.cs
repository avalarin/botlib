using System.Threading.Tasks;

namespace BotLib.Telegram.Polling {
    public interface IPollerHistoryStorage {
        Task SaveLastUpdateId(long updateId);
        Task<long> GetLastUpdateId();
    }
}