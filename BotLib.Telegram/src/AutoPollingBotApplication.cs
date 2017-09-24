using BotLib.Core.Middlewares;
using BotLib.Telegram.Polling;
using Microsoft.Extensions.Logging;

namespace BotLib.Telegram {
    public class AutoPollingBotApplication : BotApplication {
        private readonly AutoPoller _autoPoller;
        private readonly IAutoPollerConfiguration _autoPollerConfiguration;
        
        public AutoPollingBotApplication(
            IMiddlewaresChain middlewares,
            IAutoPollerConfiguration autoPollerConfiguration,
            AutoPoller autoPoller,
            ILogger<AutoPollingBotApplication> logger
        ) : base(middlewares, logger) {
            _autoPollerConfiguration = autoPollerConfiguration;
            _autoPoller = autoPoller;
            _autoPoller.UpdateReceived += (sender, eventArgs) => EnqueueUpdate(eventArgs.Update);
        }

        protected override void OnBotStarted() {
            base.OnBotStarted();
            if (_autoPollerConfiguration.Enabled) {
                _autoPoller.Start();
            }
        }

        protected override void OnBotStopping() {
            base.OnBotStopping();
            if (_autoPollerConfiguration.Enabled) {
                _autoPoller.Stop();
            }
        }
    }
}