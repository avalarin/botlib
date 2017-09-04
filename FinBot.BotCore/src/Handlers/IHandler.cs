using System.Threading.Tasks;
using FinBot.BotCore.Middlewares;

namespace FinBot.BotCore.Handlers {
    public interface IHandler {
        Task<IHandlerResult> ExecuteAsync(MiddlewareData middlewareData);
    }
}