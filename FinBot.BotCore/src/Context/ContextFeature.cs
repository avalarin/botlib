using System.Collections.Generic;
using System.Collections.Immutable;
using FinBot.BotCore.Middlewares;
using FinBot.BotCore.Utils;

namespace FinBot.BotCore.Context {
    public class ContextFeature : IFeature {
        private readonly ImmutableDictionary<string, object> _items;

        public IEnumerable<KeyValuePair<string, object>> Items => _items;
        
        public ContextFeature(IEnumerable<KeyValuePair<string, object>> items) {
            _items = items.ToImmutableDictionary();
        }
        
        private ContextFeature(ImmutableDictionary<string, object> items) {
            _items = items;
        }

        public Maybe<T> Get<T>(string key) {
            return _items.Get(key).Map(v => (T)v);
        }

        public ContextFeature Put<T>(string key, T value) {
            return new ContextFeature(_items.Add(key, value));
        }
    }
}