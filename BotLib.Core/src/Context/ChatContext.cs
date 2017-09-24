using System.Collections.Generic;

namespace BotLib.Core.Context {
    public class ChatContext : ContextWrapper {
        public ChatContext(IDictionary<string, object> items) : base(items) {
        }
    }
}