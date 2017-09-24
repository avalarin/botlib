using System.Threading.Tasks;

namespace BotLib.Core.Middlewares {
    public interface IMiddleware {
        Task<MiddlewareData> InvokeAsync(MiddlewareData data, IMiddlewaresChain chain);
    }
}