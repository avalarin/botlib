using System;
using System.Threading.Tasks;
using BotLib.Core.Middlewares;
using BotLib.Core.Security;
using BotLib.Core.Utils;
using BotLib.Telegram.Security;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BotLib.MongoDB {
    public class MongoDBAuthenticationHandler<T> : MongoStorage, IAuthenticationHandler where T : IMongoDBIdentity {
        
        private readonly IIdentiryFactory _identityFactory;

        public MongoDBAuthenticationHandler(IMongoDBConfiguration configuration, IIdentiryFactory identityFactory) : base(configuration) {
            _identityFactory = identityFactory;
        }
        
        public async Task<Maybe<IIdentity>> HandleAuthenticationAsync(MiddlewareData data, IPrincipal principal) {
            var telegramIdentity = principal.GetIdentity<TelegramIdentity>()
                .OrElseThrow(() => new InvalidOperationException("TelegramIdentity is required"));

            var telegramUserId = telegramIdentity.Id;

            var filter = Builders<T>.Filter.Eq(u => u.UserId, telegramUserId);
            var newUser = await _identityFactory.CreateIdentityAsync(principal);
            var update = new BsonDocument {{ "$setOnInsert", newUser.ToBsonDocument() }};
            
            IIdentity identity = await GetDatabase().GetCollection<T>(CollectionNames.UsersCollection)
                .FindOneAndUpdateAsync(
                    filter: filter,
                    update: update,
                    options: new FindOneAndUpdateOptions<T>() {
                        IsUpsert = true,
                        ReturnDocument = ReturnDocument.After
                    }
                );

            return identity.NotNull();
        }

    }
}