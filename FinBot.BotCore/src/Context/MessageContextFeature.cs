using System.Collections.Generic;
using System.Collections.Immutable;
using FinBot.BotCore.Middlewares;
using FinBot.BotCore.ParameterMatching;
using FinBot.BotCore.Utils;

namespace FinBot.BotCore.Context {
    public class MessageContextFeature : IFeature, IParameterValuesSource {
        private readonly ImmutableDictionary<string, object> _items;
        private readonly ImmutableHashSet<long> _messageIds;
        
        public string ChatId { get; }
        
        public IEnumerable<long> MessageIds => _messageIds;

        public IEnumerable<KeyValuePair<string, object>> Items => _items;
        
        public MessageContextFeature(string chatId, IEnumerable<long> messageIds, IEnumerable<KeyValuePair<string, object>> items) {
            ChatId = chatId;
            _messageIds = messageIds.ToImmutableHashSet();
            _items = items.ToImmutableDictionary();
        }
        
        public MessageContextFeature(string chatId, long messageId, IEnumerable<KeyValuePair<string, object>> items) {
            ChatId = chatId;
            _messageIds = ImmutableHashSet<long>.Empty.Add(messageId);
            _items = items.ToImmutableDictionary();
        }
        
        private MessageContextFeature(string chatId, ImmutableHashSet<long> messageIds, ImmutableDictionary<string, object> items) {
            ChatId = chatId;
            _messageIds = messageIds;
            _items = items;
        }

        public Maybe<T> Get<T>(string key) {
            return _items.Get(key).Map(v => (T)v);
        }

        public MessageContextFeature Put<T>(string key, T value) {
            return new MessageContextFeature(ChatId, _messageIds, _items.SetItem(key, value));
        }
        
        public MessageContextFeature PutMessageId(long messageId) {
            return new MessageContextFeature(ChatId, _messageIds.Add(messageId), _items);
        }

        public IEnumerable<ParameterValue> GetValues() {
            yield return new ParameterValue("messageContext", new MessageContext(_items));
        }
    }
}