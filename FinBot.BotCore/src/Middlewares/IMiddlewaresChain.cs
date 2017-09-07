using System.Threading.Tasks;

namespace FinBot.BotCore.Middlewares {
    public interface IMiddlewaresChain {
        Task<MiddlewareData> NextAsync(MiddlewareData data);
    }
}