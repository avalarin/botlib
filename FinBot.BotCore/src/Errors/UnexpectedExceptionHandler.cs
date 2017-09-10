using System;
using FinBot.BotCore.Middlewares;
using FinBot.BotCore.Rendering;
using Microsoft.Extensions.Logging;

namespace FinBot.BotCore.Errors {
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