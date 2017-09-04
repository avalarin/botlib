using FinBot.BotCore.Context;
using FinBot.BotCore.Telegram.Polling;
using Microsoft.Extensions.DependencyInjection;

namespace FinBot.BotCore.MongoDB {
    public static class MongoDBExtensions {

        public static IServiceCollection UseMongoDBStorages(this IServiceCollection services) {
            return services.AddSingleton<IMongoDBConfiguration, MongoDBAutoConfiguration>()
                .AddSingleton<IContextStorage, MongoDBContextStorage>()
                .AddSingleton<IPollerHistoryStorage, MongoDBPollerHistoryStorage>();
        }
        
    }
}