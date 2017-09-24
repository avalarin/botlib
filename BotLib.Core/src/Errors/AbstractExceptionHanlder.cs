using System;
using BotLib.Core.Middlewares;

namespace BotLib.Core.Errors {
    public abstract class AbstractExceptionHanlder<T> : IExceptionHandler<T> where T : Exception {
        public abstract MiddlewareData HandleException(MiddlewareData middlewareData, T exception);

        MiddlewareData IExceptionHandler.HandleException(MiddlewareData middlewareData, Exception exception) {
            return HandleException(middlewareData, (T) exception);
        }
    }
}