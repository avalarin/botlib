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

        public async Task<IEnumerable<KeyValuePair<string, object>>> LoadContext(long fromId) {
            var filter = Builders<BsonDocument>.Filter.Eq("from_id", fromId);
            var result = await GetDatabase().GetCollection<BsonDocument>(Configuration.ContextDataCollection)
                .Find(filter)
                .FirstOrDefaultAsync();

            return result.Nullable()
                .Map(GetPayload)
                .Map(ReadData)
                .OrElseGet(() => new Dictionary<string, object>());
        }

        public async Task SaveContext(long fromId, IEnumerable<KeyValuePair<string, object>> context) {
            var filter = Builders<BsonDocument>.Filter.Eq("from_id", fromId);
            var update = Builders<BsonDocument>.Update.Set("payload", new BsonDocument(context));
            
            await GetDatabase().GetCollection<BsonDocument>(Configuration.ContextDataCollection)
                .UpdateOneAsync(filter, update, new UpdateOptions() { IsUpsert = true });
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
    }
}