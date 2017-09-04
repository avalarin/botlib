using System.Threading.Tasks;
using FinBot.BotCore.Middlewares;
using FinBot.BotCore.Telegram.Features;

namespace FinBot.BotCore.Context {
    public class ContextMiddleware : IMiddleware {
        private readonly IContextStorage _storage;

        public ContextMiddleware(IContextStorage storage) {
            _storage = storage;
        }

        public async Task<MiddlewareData> InvokeAsync(MiddlewareData data, IMiddlewaresChain chain) {
            var message = data.Features.RequireOne<UpdateInfoFeature>().GetAnyMessage();
            var context = await _storage.LoadContext(message.Chat.Id);
                       
            var newData = data.UpdateFeatures(f => f.AddExclusive<ContextFeature>(new ContextFeature(context)));
            var resultData = await chain.NextAsync(newData);

            var items = resultData.Features.RequireOne<ContextFeature>().Items;
            await _storage.SaveContext(message.Chat.Id, items);
            
            return resultData;
        }
    }
}