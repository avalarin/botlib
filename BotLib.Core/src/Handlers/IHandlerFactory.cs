using System.Collections.Generic;
using BotLib.Core.Middlewares;

namespace BotLib.Core.Handlers {
    public interface IHandlerFactory {
        IEnumerable<IHandler> CreateHandlers(MiddlewareData middlewareData);
    }
}