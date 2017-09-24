using Microsoft.Extensions.Configuration;

namespace BotLib.MongoDB {
    public class MongoDBAutoConfiguration : IMongoDBConfiguration {

        private readonly IConfiguration _configuration;

        public MongoDBAutoConfiguration(IConfiguration configuration) {
            _configuration = configuration.GetSection("Mongo");
        }
        
        public string ConnectionString => _configuration["ConnectionString"];
        
        public string Database => _configuration["Database"];

    }
}