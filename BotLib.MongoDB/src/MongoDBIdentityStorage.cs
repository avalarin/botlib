﻿using System;
using System.Threading.Tasks;
using BotLib.Core.Security;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BotLib.MongoDB {
    public class MongoDBIdentityStorage : MongoStorage, IIdentityStorage {
        
        public MongoDBIdentityStorage(IMongoDBConfiguration configuration) : base(configuration) {
        }

        public async Task UpdateAsync(IIdentity identity) {
            var mongoUser = identity as IMongoDBIdentity;
            if (mongoUser == null) {
                throw new ArgumentException("identitity should be IMongoDBUser");
            }
            
            var filter = Builders<BsonDocument>.Filter.Eq("_id", mongoUser.Id);
            var update = new BsonDocument {{ "$set", mongoUser.ToBsonDocument() }};

            await GetDatabase().GetCollection<BsonDocument>(CollectionNames.UsersCollection)
                .UpdateOneAsync(
                    filter: filter,
                    update: update
                );
        }
        
    }
}