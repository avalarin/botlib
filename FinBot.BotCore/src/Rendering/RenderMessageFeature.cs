using FinBot.BotCore.Middlewares;

namespace FinBot.BotCore.Rendering {
    public class RenderMessageFeature : IFeature {

        public BaseOutMessage Message { get; }

        public RenderMessageFeature(BaseOutMessage message) {
            Message = message;
        }

    }
}