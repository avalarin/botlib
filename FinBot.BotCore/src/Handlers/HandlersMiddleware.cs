using System.Threading.Tasks;
using FinBot.BotCore.Middlewares;

namespace FinBot.BotCore.Handlers {
    public class HandlersMiddleware : IMiddleware {
        private readonly IHandlerFactory _handlerFactory;

        public HandlersMiddleware(IHandlerFactory handlerFactory) {
            _handlerFactory = handlerFactory;
        }

        public async Task<MiddlewareData> InvokeAsync(MiddlewareData data, IMiddlewaresChain chain) {
            var handler = await _handlerFactory.CreateHandlerAsync(data);
            var result = await handler.ExecuteAsync(data);
            var resultData = await result.RenderAsync(data);
            return await chain.NextAsync(resultData);
        }

    }

}