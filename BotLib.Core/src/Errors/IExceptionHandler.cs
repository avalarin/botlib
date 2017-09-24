using System;
using BotLib.Core.Middlewares;

namespace BotLib.Core.Errors {
    public interface IExceptionHandler {
        MiddlewareData HandleException(MiddlewareData middlewareData, Exception exception);
    }
    
    public interface IExceptionHandler<in T> : IExceptionHandler where T : Exception {
        MiddlewareData HandleException(MiddlewareData middlewareData, T exception);
    }
}