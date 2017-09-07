using Microsoft.Extensions.Configuration;

namespace FinBot.BotCore.MongoDB {
    public class MongoDBAutoConfiguration : IMongoDBConfiguration {

        private readonly IConfiguration _configuration;

        public MongoDBAutoConfiguration(IConfiguration configuration) {
            _configuration = configuration.GetSection("Mongo");
        }
        
        public string ConnectionString => _configuration["ConnectionString"];
        
        public string Database => _configuration["Database"];
        
        public string ContextDataCollection => _configuration["ContextDataCollection"];
        
        public string ParamsCollection => _configuration["ParamsCollection"];
        
        public string UsersCollection => _configuration["UsersCollection"];
    }
}