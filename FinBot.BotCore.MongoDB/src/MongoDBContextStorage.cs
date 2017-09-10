using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinBot.BotCore.Context;
using FinBot.BotCore.Utils;
using MongoDB.Bson;
using MongoDB.Driver;

namespace FinBot.BotCore.MongoDB {
    public class MongoDBContextStorage : MongoStorage, IContextStorage {
        
        public MongoDBContextStorage(IMongoDBConfiguration configuration) : base(configuration) {
        }

        public Task<IEnumerable<KeyValuePair<string, object>>> LoadChatContext(string chatId) {
            return LoadContext(FilterForChat(chatId));
        }

        public Task<IEnumerable<KeyValuePair<string, object>>> LoadMessageContext(string chatId, string messageId) {
            return LoadContext(FilterForMessage(chatId, messageId));
        }
        
        public Task SaveChatContext(string chatId, IEnumerable<KeyValuePair<string, object>> context) {
            return SaveContext(FilterForChat(chatId), context);
        }

        public Task SaveMessageContext(string chatId, string messageId, IEnumerable<KeyValuePair<string, object>> context) {
            return SaveContext(FilterForMessage(chatId, messageId), context);
        }

        private async Task<IEnumerable<KeyValuePair<string, object>>> LoadContext(FilterDefinition<BsonDocument> filter) {
            var result = await GetDatabase().GetCollection<BsonDocument>(Configuration.ContextDataCollection)
                .Find(filter)
                .FirstOrDefaultAsync();

            return result.Nullable()
                .Map(GetPayload)
                .Map(ReadData)
                .OrElseGet(() => new Dictionary<string, object>());
        }
        
        private Task SaveContext(FilterDefinition<BsonDocument> filter, IEnumerable<KeyValuePair<string, object>> context) {
            var items = context.ToList();
            
            if (!items.Any()) {
                return GetDatabase().GetCollection<BsonDocument>(Configuration.ContextDataCollection)
                    .DeleteOneAsync(filter);
            }
            
            return GetDatabase().GetCollection<BsonDocument>(Configuration.ContextDataCollection)
                .UpdateOneAsync(
                    filter: filter, 
                    update: Builders<BsonDocument>.Update.Set("payload", new BsonDocument(items)),
                    options: new UpdateOptions() { IsUpsert = true }
                );
        }
        
        private Maybe<BsonDocument> GetPayload(BsonDocument document) {
            if (document.TryGetValue("payload", out var payload)) {
                return payload.AsBsonDocument.NotNull();
            }
            return Maybe<BsonDocument>.Empty();
        }
        
        private IEnumerable<KeyValuePair<string, object>> ReadData(BsonDocument payload) {
            return payload.Select(element => new KeyValuePair<string, object>(element.Name, BsonTypeMapper.MapToDotNetValue(element.Value)));
        }
        
        private static FilterDefinition<BsonDocument> FilterForChat(string chatId) {
            return Builders<BsonDocument>.Filter.And(
                Builders<BsonDocument>.Filter.Eq("chat_id", chatId),
                Builders<BsonDocument>.Filter.Eq("context", "chat")
            );
        }

        private static FilterDefinition<BsonDocument> FilterForMessage(string chatId, string messageId) {
            return Builders<BsonDocument>.Filter.And(
                Builders<BsonDocument>.Filter.Eq("chat_id", chatId),
                Builders<BsonDocument>.Filter.Eq("context", "message"),
                Builders<BsonDocument>.Filter.Eq("message_id", messageId)
            );
        }
    }
}