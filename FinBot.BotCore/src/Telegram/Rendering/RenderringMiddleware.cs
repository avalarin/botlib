using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FinBot.BotCore.Middlewares;
using FinBot.BotCore.Telegram.Client;
using FinBot.BotCore.Telegram.Features;
using FinBot.BotCore.Utils;

namespace FinBot.BotCore.Telegram.Rendering {
    public class RenderringMiddleware : IMiddleware {
        private readonly ITelegramClient _client;

        public RenderringMiddleware(ITelegramClient client) {
            _client = client;
        }

        public async Task<MiddlewareData> InvokeAsync(MiddlewareData data, IMiddlewaresChain chain) {
            var resultData = await chain.NextAsync(data);
            foreach (var renderer in resultData.Features.GetAll<ClientRendererFeature>().Select(f => f.Renderer)) {
                resultData = await renderer.Render(resultData, _client, CancellationToken.None);
            }
            return resultData;
        }
        
    }
}