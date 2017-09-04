using System.Threading.Tasks;

namespace FinBot.BotCore.Middlewares {
    public interface IMiddleware {
        Task<MiddlewareData> InvokeAsync(MiddlewareData data, IMiddlewaresChain chain);
    }
}