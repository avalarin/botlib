using FinBot.BotCore.Security;
using MongoDB.Bson;

namespace FinBot.BotCore.MongoDB {
    public interface IMongoDBIdentity : IIdentity {
        
        ObjectId Id { get; }
        
        string UserId { get; }
        
    }
}