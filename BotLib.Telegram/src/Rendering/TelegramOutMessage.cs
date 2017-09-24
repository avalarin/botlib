using BotLib.Core.Rendering;
using BotLib.Telegram.Models;

namespace BotLib.Telegram.Rendering {
    public class TelegramOutMessage : BaseOutMessage {
        public MessageContent MessageContent { get; }
        
        public string ChatId { get; set; }
        
        public long? MessageId { get; set; }
        
        public bool UpdateMessage { get; set; }

        public TelegramOutMessage(MessageContent messageContent) {
            MessageContent = messageContent;
        }
    }
}