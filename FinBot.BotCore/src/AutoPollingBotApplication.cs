using System;
using FinBot.BotCore.Middlewares;
using FinBot.BotCore.Telegram.Polling;
using FinBot.BotCore.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace FinBot.BotCore {
    public class AutoPollingBotApplication : BotApplication {

        private readonly AutoPoller _autoPoller;
        private readonly IAutoPollerConfiguration _autoPollerConfiguration;
        
        public AutoPollingBotApplication(IServiceProvider services, IMiddlewaresChain middlewares) : base(services, middlewares) {
            _autoPollerConfiguration = services.GetService<IAutoPollerConfiguration>();
            _autoPoller = services.GetInstance<AutoPoller>();
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