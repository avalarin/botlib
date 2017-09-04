using System.Threading.Tasks;
using FinBot.BotCore.Middlewares;

namespace FinBot.BotCore.Handlers {
    public interface IHandlerResult {
        Task<MiddlewareData> RenderAsync(MiddlewareData data);
    }
}