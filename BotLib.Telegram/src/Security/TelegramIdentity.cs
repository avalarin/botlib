using BotLib.Core.Security;

namespace BotLib.Telegram.Security {
    public class TelegramIdentity : IIdentity {
        
        public bool Is​Authenticated => false;
        
        public string Id { get; }
        
        public string Name { get; }

        public TelegramIdentity(string id, string name) {
            Id = id;
            Name = name;
        }
    }
}