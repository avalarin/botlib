using System.Threading.Tasks;

namespace FinBot.BotCore.Telegram.Polling {
    public interface IPollerHistoryStorage {
        Task SaveLastUpdateId(long updateId);
        Task<long> GetLastUpdateId();
    }
}