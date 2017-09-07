﻿using FinBot.BotCore.Context;
using FinBot.BotCore.Security;
using FinBot.BotCore.Telegram.Polling;
using Microsoft.Extensions.DependencyInjection;

namespace FinBot.BotCore.MongoDB {
    public static class MongoDBExtensions {

        public static IServiceCollection UseMongoDBStorages(this IServiceCollection services) {
            return services.AddSingleton<IMongoDBConfiguration, MongoDBAutoConfiguration>()
                .AddSingleton<IContextStorage, MongoDBContextStorage>()
                .AddSingleton<IPollerHistoryStorage, MongoDBPollerHistoryStorage>();
        }

        public static IServiceCollection UseMongoDBAuthentication<T, TProvider>(this IServiceCollection services)
            where T : IMongoDBIdentity
            where TProvider : class, IIdentiryFactory {

            return services.AddSingleton<IAuthenticationHandler, MongoDBAuthenticationHandler<T>>()
                .AddSingleton<IIdentiryFactory, TProvider>()
                .AddSingleton<IIdentityStorage, MongoDBIdentityStorage>();
        }
        
    }
}