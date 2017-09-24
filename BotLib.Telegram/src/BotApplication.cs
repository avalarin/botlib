using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using BotLib.Core.Middlewares;
using BotLib.Core.Utils;
using BotLib.Telegram.Features;
using BotLib.Telegram.Models;
using Microsoft.Extensions.Logging;

namespace BotLib.Telegram {
    public class BotApplication {
        private readonly ConcurrentQueue<UpdateInfo> _updatesQueue;
        private readonly ILogger _logger;
        private readonly IMiddlewaresChain _middlewares;
        private readonly object _lock = new object();

        private bool _started = false;
        private CancellationTokenSource _cancellationTokenSource;
        private Task _runningTask;

        public BotApplication(IMiddlewaresChain middlewares, ILogger<BotApplication> logger) {
            _updatesQueue = new ConcurrentQueue<UpdateInfo>();
            _middlewares = middlewares;
            _logger = logger;
        }

        public void Start() {
            if (_started) throw new InvalidOperationException("Application already started");
            lock (_lock) {
                if (_started) throw new InvalidOperationException("Application already started");
                _cancellationTokenSource = new CancellationTokenSource();

                try {
                    _runningTask = ProcessUpdates(_cancellationTokenSource.Token);

                    _logger.LogInformation("Application started...");
                    
                    OnBotStarted();

                    _started = true;
                }
                catch (Exception e) {
                    _logger.LogError(null, e, "Cannot start application");
                    throw;
                }
            }
        }

        public Task StopAsync() {
            if (!_started) throw new InvalidOperationException("Application is not running");
            lock (_lock) {
                if (!_started) throw new InvalidOperationException("Application is not running");

                OnBotStopping();
                
                _cancellationTokenSource.Cancel();

                return _runningTask;
            }
        }

        public void StartAndLock() {
            Start();
            
            Console.Write("Press any key to stop the application");
            Console.ReadKey();

            StopAsync().Wait();
        }
        
        public void EnqueueUpdate(UpdateInfo updateInfo) {
            _logger.LogTrace($"Update #{updateInfo.Id} enqueued");
            _updatesQueue.Enqueue(updateInfo);
        }

        protected virtual void OnBotStarted() { }
        
        protected virtual void OnBotStopping() { }
        
        private async Task ProcessUpdates(CancellationToken token) {
            await Task.Yield();

            while (!token.IsCancellationRequested) {
                UpdateInfo updateInfo;
                if (!_updatesQueue.TryDequeue(out updateInfo)) {
                    await Task.Delay(TimeSpan.FromSeconds(1), token);
                    continue;
                }

                try {
                    var inputData = new MiddlewareData()
                        .UpdateFeatures(f => f.AddExclusive<UpdateInfoFeature>(new UpdateInfoFeature(updateInfo)));

                    await _middlewares.NextAsync(inputData);
                }
                catch (Exception e) {
                    _logger.LogError(0, e, "Unhandled exception occured");
                }
            }
        }

        public static BotApplication Create(IServiceProvider services) {
            return services.GetInstance<AutoPollingBotApplication>();
        }
        
    }
}