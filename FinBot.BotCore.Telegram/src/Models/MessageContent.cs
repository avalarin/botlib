namespace FinBot.BotCore.Telegram.Models {
    public class MessageContent {

        public string Text { get; set; }

        public InlineKeyboardMarkup ReplyMarkup { get; set; }

        public MessageContent() {
        }

        public MessageContent(string text) {
            Text = text;
        }

    }
}