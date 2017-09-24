using System;

namespace BotLib.Core.Middlewares {
    public interface IMiddlewareHolder {
        int Order { get; }
        
        MiddlewaresChainBuilder AppendMiddlewares(MiddlewaresChainBuilder chain);
    }

    public class MiddlewareHolder : IMiddlewareHolder {
        private readonly Func<MiddlewaresChainBuilder, MiddlewaresChainBuilder> _modifier;

        public MiddlewareHolder(int order, Func<MiddlewaresChainBuilder, MiddlewaresChainBuilder> modifier) {
            Order = order;
            _modifier = modifier;
        }

        public int Order { get; }

        public MiddlewaresChainBuilder AppendMiddlewares(MiddlewaresChainBuilder chain) {
            return _modifier(chain);
        }
    }
}