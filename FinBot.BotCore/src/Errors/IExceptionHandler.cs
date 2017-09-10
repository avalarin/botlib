using System;
using FinBot.BotCore.Middlewares;

namespace FinBot.BotCore.Errors {
    public interface IExceptionHandler {
        MiddlewareData HandleException(MiddlewareData middlewareData, Exception exception);
    }
    
    public interface IExceptionHandler<in T> : IExceptionHandler where T : Exception {
        MiddlewareData HandleException(MiddlewareData middlewareData, T exception);
    }
}