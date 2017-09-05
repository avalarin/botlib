using System.Collections.Generic;
using System.Threading.Tasks;
using FinBot.BotCore.Middlewares;

namespace FinBot.BotCore.Handlers {
    public interface IHandlerFactory {
        IEnumerable<IHandler> CreateHandlers(MiddlewareData middlewareData);
    }
}