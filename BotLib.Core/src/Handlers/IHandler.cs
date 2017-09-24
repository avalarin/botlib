using System.Threading.Tasks;
using BotLib.Core.Middlewares;

namespace BotLib.Core.Handlers {
    public interface IHandler {
        Task<IHandlerResult> ExecuteAsync(MiddlewareData middlewareData);
    }
}