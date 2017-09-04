using MongoDB.Driver;

namespace FinBot.BotCore.MongoDB {
    public class MongoStorage {
        protected IMongoDBConfiguration Configuration { get; }

        public MongoStorage(IMongoDBConfiguration configuration) {
            Configuration = configuration;
        }

        protected IMongoDatabase GetDatabase() {
            return new MongoClient(Configuration.ConnectionString)
                .GetDatabase(Configuration.Database);
        }
    }
}