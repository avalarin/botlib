using System;
using System.Threading.Tasks;
using FinBot.BotCore.Exceptions;
using FinBot.BotCore.Handlers;
using FinBot.BotCore.Middlewares;
using FinBot.BotCore.Telegram.Features;
using FinBot.BotCore.Telegram.Models;
using FinBot.BotCore.Telegram.Rendering;
using Microsoft.Extensions.Logging;

namespace FinBot.BotCore.Telegram.Middlewares {
    public class HandleErrorMiddleware : IMiddleware {
        private readonly ILogger _logger;

        public HandleErrorMiddleware(ILoggerFactory loggerFactory) {
            _logger = loggerFactory.CreateLogger(GetType());
        }

        public async Task<MiddlewareData> InvokeAsync(MiddlewareData data, IMiddlewaresChain chain) {
            try {
                return await chain.NextAsync(data);
            }
            catch (Exception e) {
                return await MapException(e).RenderAsync(data);
            }
        }

        private IHandlerResult MapException(Exception e) {
            if (e is NoSuchHandlerException noSuchHandlerException) {
                _logger.LogInformation("Cannot find handler for message");
                return HandlerResult.WithText("Unknown command");
            }
            _logger.LogError(0, e, "Unexpected error occurred");
            return HandlerResult.WithText($"Error occured: [{e.GetType().Name}] {e.Message}");
        }
    }
}