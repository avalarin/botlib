using System;
using BotLib.Core.Utils;
using Microsoft.Extensions.Configuration;

namespace BotLib.Telegram.Polling {
    public class AutoPollerAutoConfiguration : IAutoPollerConfiguration {

        private readonly IConfiguration _configuration;

        public AutoPollerAutoConfiguration(IConfiguration configuration) {
            _configuration = configuration.GetSection("Telegram");
        }

        public bool Enabled => _configuration.Get<bool?>("AutoPoller:Enabled") ?? false;

        public int? OneTimeLimit => _configuration.Get<int?>("AutoPoller:OneTimeLimit");

        public TimeSpan? PoolingTimeout => _configuration.Get<TimeSpan?>("AutoPoller:PoolingTimeout", str => TimeSpan.Parse(str));

        public string[] FieldsFilter => null;

    }
}