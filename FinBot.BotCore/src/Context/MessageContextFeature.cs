using System.Collections.Generic;
using System.Collections.Immutable;
using FinBot.BotCore.Middlewares;
using FinBot.BotCore.ParameterMatching;
using FinBot.BotCore.Utils;

namespace FinBot.BotCore.Context {
    public class MessageContextFeature : IFeature, IParameterValuesSource {
        private readonly ImmutableDictionary<string, object> _items;
        private readonly ImmutableHashSet<string> _messageIds;
        
        public IEnumerable<string> MessageIds => _messageIds;

        public IEnumerable<KeyValuePair<string, object>> Items => _items;
        
        public MessageContextFeature(IEnumerable<string> messageIds, IEnumerable<KeyValuePair<string, object>> items) {
            _messageIds = messageIds.ToImmutableHashSet();
            _items = items.ToImmutableDictionary();
        }
        
        public MessageContextFeature(string messageId, IEnumerable<KeyValuePair<string, object>> items) {
            _messageIds = ImmutableHashSet<string>.Empty.Add(messageId);
            _items = items.ToImmutableDictionary();
        }
        
        private MessageContextFeature(ImmutableHashSet<string> messageIds, ImmutableDictionary<string, object> items) {
            _messageIds = messageIds;
            _items = items;
        }

        public Maybe<T> Get<T>(string key) {
            return _items.Get(key).Map(v => (T)v);
        }

        public MessageContextFeature Put<T>(string key, T value) {
            return new MessageContextFeature(_messageIds, _items.SetItem(key, value));
        }
        
        public MessageContextFeature PutMessageId(string messageId) {
            return new MessageContextFeature(_messageIds.Add(messageId), _items);
        }

        public IEnumerable<ParameterValue> GetValues() {
            yield return new ParameterValue("messageContext", new MessageContext(_items));
        }
    }
}