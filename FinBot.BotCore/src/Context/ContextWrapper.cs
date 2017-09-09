using System.Collections.Generic;
using FinBot.BotCore.Utils;

namespace FinBot.BotCore.Context {
    public abstract class ContextWrapper {
        private readonly IDictionary<string, object> _items;

        public ContextWrapper(IDictionary<string, object> items) {
            _items = items;
        }

        public Maybe<T> Get<T>(string key) {
            return _items.Get(key)
                .Map(value => (T)value);
        }
    }
}