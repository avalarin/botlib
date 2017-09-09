using System.Collections.Generic;

namespace FinBot.BotCore.Context {
    public class ChatContext : ContextWrapper {
        public ChatContext(IDictionary<string, object> items) : base(items) {
        }
    }
}