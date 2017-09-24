using System.Threading.Tasks;

namespace BotLib.Core.Middlewares {
    public interface IMiddlewaresChain {
        Task<MiddlewareData> NextAsync(MiddlewareData data);
    }
}