using System.Collections.Generic;
using System.Collections.Immutable;
using FinBot.BotCore.Middlewares;
using FinBot.BotCore.ParameterMatching;
using FinBot.BotCore.Utils;

namespace FinBot.BotCore.Context {
    public class ChatContextFeature : IFeature, IParameterValuesSource {
        private readonly ImmutableDictionary<string, object> _items;

        public IEnumerable<KeyValuePair<string, object>> Items => _items;
        
        public ChatContextFeature(IEnumerable<KeyValuePair<string, object>> items) {
            _items = items.ToImmutableDictionary();
        }
        
        private ChatContextFeature(ImmutableDictionary<string, object> items) {
            _items = items;
        }

        public Maybe<T> Get<T>(string key) {
            return _items.Get(key).Map(v => (T)v);
        }

        public ChatContextFeature Put<T>(string key, T value) {
            return new ChatContextFeature(_items.SetItem(key, value));
        }
        
        public IEnumerable<ParameterValue> GetValues() {
            yield return new ParameterValue("chatContext", new MessageContext(_items));
        }
    }
}