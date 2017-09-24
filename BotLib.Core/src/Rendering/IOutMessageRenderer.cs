using System.Threading.Tasks;
using BotLib.Core.Middlewares;

namespace BotLib.Core.Rendering {
    public interface IOutMessageRenderer {
        Task<MiddlewareData> RenderAsync(MiddlewareData data, BaseOutMessage message);
    }
    
    public interface IOutMessageRenderer<in T> : IOutMessageRenderer where T : BaseOutMessage {
        Task<MiddlewareData> RenderAsync(MiddlewareData data, T message);
    }
}