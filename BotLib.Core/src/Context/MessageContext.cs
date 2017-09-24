using System.Collections.Generic;

namespace BotLib.Core.Context {
    public class MessageContext : ContextWrapper {
        public MessageContext(IDictionary<string, object> items) : base(items) {
        }
    }
}