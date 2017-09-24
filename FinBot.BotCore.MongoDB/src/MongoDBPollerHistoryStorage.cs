using System.Threading.Tasks;
using FinBot.BotCore.Telegram.Polling;
using MongoDB.Bson;
using MongoDB.Driver;

namespace FinBot.BotCore.MongoDB {
    public class MongoDBPollerHistoryStorage : MongoStorage, IPollerHistoryStorage {
        
        public MongoDBPollerHistoryStorage(IMongoDBConfiguration configuration) : base(configuration) {
        }

        public async Task SaveLastUpdateId(long updateId) {
            var filter = Builders<BsonDocument>.Filter.Eq("key", "last_update_id");
            var update = Builders<BsonDocument>.Update.Set("value", updateId);
            
            await GetDatabase().GetCollection<BsonDocument>(CollectionNames.ParamsCollection)
                .UpdateOneAsync(filter, update, new UpdateOptions() { IsUpsert = true });
        }

        public async Task<long> GetLastUpdateId() {
            var filter = Builders<BsonDocument>.Filter.Eq("key", "last_update_id");
            var result = await GetDatabase().GetCollection<BsonDocument>(CollectionNames.ParamsCollection)
                .Find(filter)
                .FirstOrDefaultAsync();
            return result?.GetElement("value").Value.AsInt64 ?? 0;
        }
        
    }
}