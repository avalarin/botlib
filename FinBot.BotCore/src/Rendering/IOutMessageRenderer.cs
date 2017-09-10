using System.Threading.Tasks;
using FinBot.BotCore.Middlewares;

namespace FinBot.BotCore.Rendering {
    public interface IOutMessageRenderer {
        Task<MiddlewareData> RenderAsync(MiddlewareData data, BaseOutMessage message);
    }
    
    public interface IOutMessageRenderer<in T> : IOutMessageRenderer where T : BaseOutMessage {
        Task<MiddlewareData> RenderAsync(MiddlewareData data, T message);
    }
}