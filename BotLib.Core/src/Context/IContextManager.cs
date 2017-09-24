using BotLib.Core.Middlewares;

namespace BotLib.Core.Context {
    public interface IContextManager {

        string GetChatId(MiddlewareData middlewareData);
        
        string GetMessageId(MiddlewareData middlewareData);
        
        bool HasMessageContext(MiddlewareData middlewareData);

    }
}