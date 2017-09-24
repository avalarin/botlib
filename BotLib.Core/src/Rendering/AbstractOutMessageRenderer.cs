using System.Threading.Tasks;
using BotLib.Core.Middlewares;

namespace BotLib.Core.Rendering {
    public abstract class AbstractOutMessageRenderer<T> : IOutMessageRenderer<T> where T : BaseOutMessage {
        Task<MiddlewareData> IOutMessageRenderer.RenderAsync(MiddlewareData data, BaseOutMessage message) {
            return RenderAsync(data, (T)message);
        }

        public abstract Task<MiddlewareData> RenderAsync(MiddlewareData data, T message);
    }
}