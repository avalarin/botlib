using BotLib.Core.Security;
using MongoDB.Bson;

namespace BotLib.MongoDB {
    public interface IMongoDBIdentity : IIdentity {
        ObjectId Id { get; }
        string UserId { get; } 
    }
}