using BotLib.Core.Middlewares;

namespace BotLib.Core.Rendering {
    public class RenderMessageFeature : IFeature {

        public BaseOutMessage Message { get; }

        public RenderMessageFeature(BaseOutMessage message) {
            Message = message;
        }

    }
}