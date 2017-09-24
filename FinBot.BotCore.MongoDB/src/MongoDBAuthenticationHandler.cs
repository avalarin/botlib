using System;
using System.Threading.Tasks;
using FinBot.BotCore.Middlewares;
using FinBot.BotCore.Security;
using FinBot.BotCore.Telegram.Security;
using FinBot.BotCore.Utils;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace FinBot.BotCore.MongoDB {
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