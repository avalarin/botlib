using FinBot.BotCore.Rendering;
using FinBot.BotCore.Telegram.Models;

namespace FinBot.BotCore.Telegram.Rendering {
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