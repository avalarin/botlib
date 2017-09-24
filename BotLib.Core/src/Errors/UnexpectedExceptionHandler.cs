using System;
using BotLib.Core.Middlewares;
using BotLib.Core.Rendering;
using Microsoft.Extensions.Logging;

namespace BotLib.Core.Errors {
    public class UnexpectedExceptionHandler : AbstractExceptionHanlder<Exception> {
        private readonly ILogger _logger;
        
        public UnexpectedExceptionHandler(ILoggerFactory loggerFactory) {
            _logger = loggerFactory.CreateLogger<UnexpectedExceptionHandler>();
        }

        public override MiddlewareData HandleException(MiddlewareData middlewareData, Exception exception) {
            _logger.LogError(0, exception, "Unexpected error occurred");
            var message = new BaseOutMessage() {
                Text = $"Error occured: [{exception.GetType().Name}] {exception.Message}"
            };
            return middlewareData.AddRenderMessageFeature(message);
        }

    }
}