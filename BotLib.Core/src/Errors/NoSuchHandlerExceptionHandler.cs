using BotLib.Core.Exceptions;
using BotLib.Core.Middlewares;
using BotLib.Core.Rendering;
using Microsoft.Extensions.Logging;

namespace BotLib.Core.Errors {
    public class NoSuchHandlerExceptionHandler : AbstractExceptionHanlder<NoSuchHandlerException> {
        private readonly ILogger _logger;
        
        public NoSuchHandlerExceptionHandler(ILoggerFactory loggerFactory) {
            _logger = loggerFactory.CreateLogger<UnexpectedExceptionHandler>();
        }

        public override MiddlewareData HandleException(MiddlewareData middlewareData, NoSuchHandlerException exception) {
            _logger.LogInformation("Cannot find handler for message");
            var message = new BaseOutMessage() {
                Text = "Unknown command"
            };
            return middlewareData.AddRenderMessageFeature(message);
        }
        
    }
}