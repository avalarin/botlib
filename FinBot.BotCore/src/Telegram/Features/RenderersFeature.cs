using FinBot.BotCore.Middlewares;
using FinBot.BotCore.Telegram.Rendering;

namespace FinBot.BotCore.Telegram.Features {
    public class ClientRendererFeature : IFeature {

        public IClientRenderer Renderer { get; }

        public ClientRendererFeature(IClientRenderer renderer) {
            Renderer = renderer;
        }

    }
}