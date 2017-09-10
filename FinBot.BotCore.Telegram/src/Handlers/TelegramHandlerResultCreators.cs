using FinBot.BotCore.Handlers;

namespace FinBot.BotCore.Telegram.Handlers {
    public static class TelegramHandlerResultCreators {

        public static IHandlerResult Text(string text) {
            return new HandlerResultBuilder()
                .Text(text)
                .Create();
        }
        
    }
}