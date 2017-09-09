using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinBot.BotCore.Context {
    public interface IContextStorage {

        Task<IEnumerable<KeyValuePair<string, object>>> LoadChatContext(long fromId);
        
        Task<IEnumerable<KeyValuePair<string, object>>> LoadMessageContext(long fromId, long messageId);

        Task SaveChatContext(long fromId, IEnumerable<KeyValuePair<string, object>> context);

        Task SaveMessageContext(long fromId, long messageId, IEnumerable<KeyValuePair<string, object>> context);
        
    }
}
