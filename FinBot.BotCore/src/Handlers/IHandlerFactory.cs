using System.Threading.Tasks;
using FinBot.BotCore.Middlewares;

namespace FinBot.BotCore.Handlers {
    public interface IHandlerFactory {
        Task<IHandler> CreateHandlerAsync(MiddlewareData middlewareData);
    }
}