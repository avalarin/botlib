using Newtonsoft.Json;

namespace FinBot.BotCore.Telegram.Models {
    public class UpdateMessageData : SendMessageData {
        public UpdateMessageData() {
        }

        public UpdateMessageData(string chatId, long messageId, MessageContent messageContent) : base(chatId, messageContent) {
            MessageId = messageId;
        }

        [JsonProperty(PropertyName = "message_id")]
        public long MessageId { get; set; }

    }
}