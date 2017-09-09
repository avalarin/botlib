using System.Collections.Generic;

namespace FinBot.BotCore.Context {
    public class MessageContext : ContextWrapper {
        public MessageContext(IDictionary<string, object> items) : base(items) {
        }
    }
}