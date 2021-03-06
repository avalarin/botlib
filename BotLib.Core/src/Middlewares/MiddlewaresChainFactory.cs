﻿using System;
using System.Diagnostics;
using System.Threading.Tasks;
using BotLib.Core.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BotLib.Core.Middlewares {
    public class MiddlewaresChainFactory : IMiddlewaresChainFactory {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger _logger;

        public MiddlewaresChainFactory(IServiceProvider serviceProvider) {
            _serviceProvider = serviceProvider;
            _logger = serviceProvider.GetService<ILoggerFactory>().CreateLogger("middlewares");
        }

        public IMiddlewaresChain CreateElement(Type type, IMiddlewaresChain next) {
            return new ChainElement(_logger, _serviceProvider, type, next);
        }

        public IMiddlewaresChain CreateElement(IMiddleware instance, IMiddlewaresChain next) {
            return new ChainElement(_logger, _serviceProvider, instance, next);
        }

        public IMiddlewaresChain CreateTailElement() {
            return new TailElement();
        }

        private class ChainElement : IMiddlewaresChain {
            private readonly ILogger _logger;
            private readonly IServiceProvider _serviceProvider;
            private readonly IMiddleware _instance;
            private readonly Type _type;
            private readonly IMiddlewaresChain _next;

            public ChainElement(ILogger logger, IServiceProvider serviceProvider, IMiddleware instance, IMiddlewaresChain next) {
                _logger = logger;
                _serviceProvider = serviceProvider;
                _instance = instance;
                _type = null;
                _next = next;
            }

            public ChainElement(ILogger logger, IServiceProvider serviceProvider, Type type, IMiddlewaresChain next) {
                _logger = logger;
                _serviceProvider = serviceProvider;
                _instance = null;
                _type = type;
                _next = next;
            }

            public async Task<MiddlewareData> NextAsync(MiddlewareData data) {
                var sw = new Stopwatch();
                sw.Start();
                var result = await GetInstance().InvokeAsync(data, new TimerChainElement(sw, _next));
                sw.Stop();

                return result;
            }
            
            private IMiddleware GetInstance() {
                if (_instance != null) {
                    return _instance;
                }
                return (IMiddleware)_serviceProvider.GetInstance(_type);
            }
        }

        private class TimerChainElement : IMiddlewaresChain {
            private readonly Stopwatch _stopwatch;
            private readonly IMiddlewaresChain _next;

            public TimerChainElement(Stopwatch stopwatch, IMiddlewaresChain next) {
                _stopwatch = stopwatch;
                _next = next;
            }

            public async Task<MiddlewareData> NextAsync(MiddlewareData data) {
                _stopwatch.Stop();
                var result = await _next.NextAsync(data);
                _stopwatch.Start();
                return result;
            }
        }

        private class TailElement : IMiddlewaresChain {
            public Task<MiddlewareData> NextAsync(MiddlewareData data) {
                return Task.FromResult(data);
            }
        }
    }
}