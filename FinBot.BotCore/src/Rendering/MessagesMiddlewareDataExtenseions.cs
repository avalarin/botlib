using FinBot.BotCore.Middlewares;

namespace FinBot.BotCore.Rendering {
    public static class MessagesMiddlewareDataExtenseions {

        public static MiddlewareData AddRenderMessageFeature(this MiddlewareData middlewareData, BaseOutMessage message) {
            return middlewareData.UpdateFeatures(f => f.Add<RenderMessageFeature>(new RenderMessageFeature(message)));
        }
        
    }
}