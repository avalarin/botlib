using System.Collections.Generic;
using System.Threading.Tasks;

namespace BotLib.Core.Context {
    public interface IContextStorage {

        Task<IEnumerable<KeyValuePair<string, object>>> LoadChatContext(string chatId);
        
        Task<IEnumerable<KeyValuePair<string, object>>> LoadMessageContext(string chatId, string messageId);

        Task SaveChatContext(string chatId, IEnumerable<KeyValuePair<string, object>> context);

        Task SaveMessageContext(string chatId, string messageId, IEnumerable<KeyValuePair<string, object>> context);
        
    }
}
