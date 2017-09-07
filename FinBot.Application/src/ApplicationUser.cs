using System;
using FinBot.BotCore.MongoDB;
using FinBot.BotCore.Security;
using FinBot.BotCore.Telegram.Security;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FinBot.Application {
    [BsonIgnoreExtraElements]
    public class ApplicationUser : IMongoDBIdentity {
        [BsonId]
        public ObjectId Id { get; }
        
        public bool Is​Authenticated { get; }
        
        public string UserId { get; }
        
        public string UserName { get; }
        
        public DateTime CreatedAt { get; }
        
        public DateTime UpdatedAt { get; }

        [BsonConstructor]
        public ApplicationUser(ObjectId id, bool is​Authenticated, string userId, string userName, DateTime createdAt, DateTime updatedAt) {
            Id = id;
            Is​Authenticated = is​Authenticated;
            UserId = userId;
            UserName = userName;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }
        
        public ApplicationUser Authenticated() {
            return new ApplicationUser(Id, true, UserId, UserName, CreatedAt, DateTime.Now);
        }
        
        public ApplicationUser Unauthenticated() {
            return new ApplicationUser(Id, false, UserId, UserName, CreatedAt, DateTime.Now);
        }

        public static ApplicationUser Create(TelegramIdentity telegramIdentity) {
            return new ApplicationUser(ObjectId.GenerateNewId(), false, telegramIdentity.Id, telegramIdentity.Name, DateTime.Now, DateTime.Now);
        }
    }
}