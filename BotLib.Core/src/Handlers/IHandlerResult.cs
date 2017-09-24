using System.Threading.Tasks;
using BotLib.Core.Middlewares;

namespace BotLib.Core.Handlers {
    public interface IHandlerResult {
        Task<MiddlewareData> RenderAsync(MiddlewareData data);
    }
}