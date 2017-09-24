using System;

namespace BotLib.Core.Middlewares {
    public interface IMiddlewaresChainFactory {

        IMiddlewaresChain CreateElement(Type type, IMiddlewaresChain next);

        IMiddlewaresChain CreateElement(IMiddleware instance, IMiddlewaresChain next);

        IMiddlewaresChain CreateTailElement();

    }
}