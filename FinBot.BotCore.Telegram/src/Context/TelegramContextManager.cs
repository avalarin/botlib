using FinBot.BotCore.Context;
using FinBot.BotCore.Middlewares;
using FinBot.BotCore.Telegram.Features;

namespace FinBot.BotCore.Telegram.Context {
    public class TelegramContextManager : IContextManager {
        public string GetChatId(MiddlewareData middlewareData) {
            return middlewareData.Features.RequireOne<UpdateInfoFeature>()
                .GetAnyMessage().Chat.Id.ToString();
        }

        public string GetMessageId(MiddlewareData middlewareData) {
            return middlewareData.Features.RequireOne<UpdateInfoFeature>()
                .GetAnyMessage().Id.ToString();
        }

        public bool HasMessageContext(MiddlewareData middlewareData) {
            var updateInfo = middlewareData.Features.RequireOne<UpdateInfoFeature>();
            var contextMessageId = updateInfo.Update.EditedMessage?.Id
                                ?? updateInfo.Update.EditedChannelPost?.Id
                                ?? updateInfo.Update.CallbackQuery?.Message.Id;
            return contextMessageId != null;
        }
    }
}