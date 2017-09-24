using BotLib.Core.Handlers;

namespace BotLib.Telegram.Handlers {
    public static class TelegramHandlerResultCreators {

        public static IHandlerResult Text(string text) {
            return new HandlerResultBuilder()
                .Text(text)
                .Create();
        }
        
    }
}