using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinBot.BotCore.Context {
    public interface IContextStorage {

        Task<IEnumerable<KeyValuePair<string, object>>> LoadContext(long fromId);

        Task SaveContext(long fromId, IEnumerable<KeyValuePair<string, object>> context);

    }
}
