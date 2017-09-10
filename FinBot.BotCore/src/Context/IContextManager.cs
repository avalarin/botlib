using FinBot.BotCore.Middlewares;

namespace FinBot.BotCore.Context {
    public interface IContextManager {

        string GetChatId(MiddlewareData middlewareData);
        
        string GetMessageId(MiddlewareData middlewareData);
        
        bool HasMessageContext(MiddlewareData middlewareData);

    }
}