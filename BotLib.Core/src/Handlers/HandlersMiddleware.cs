using System.Threading.Tasks;
using BotLib.Core.Exceptions;
using BotLib.Core.Middlewares;

namespace BotLib.Core.Handlers {
    public class HandlersMiddleware : IMiddleware {
        private readonly IHandlerFactory _handlerFactory;

        public HandlersMiddleware(IHandlerFactory handlerFactory) {
            _handlerFactory = handlerFactory;
        }

        public async Task<MiddlewareData> InvokeAsync(MiddlewareData data, IMiddlewaresChain chain) {
            foreach (var handler in _handlerFactory.CreateHandlers(data)) {
                var result = await handler.ExecuteAsync(data);
                if (result == null) { // TODO fix this check
                    continue;
                }
                var resultData = await result.RenderAsync(data);
                return await chain.NextAsync(resultData);
            }
            
            throw new NoSuchHandlerException();
        }

    }

}