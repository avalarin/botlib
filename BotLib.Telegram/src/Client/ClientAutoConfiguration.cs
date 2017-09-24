using System;
using BotLib.Core.Utils;
using Microsoft.Extensions.Configuration;

namespace BotLib.Telegram.Client {
    public class ClientAutoConfiguration : IClientConfiguration {
        private readonly IConfiguration _configuration;

        public ClientAutoConfiguration(IConfiguration configuration) {
            _configuration = configuration.GetSection("Telegram:Client");
        }

        public string Token => _configuration["Token"];

        public TimeSpan? RequestTimeout => _configuration.Get<TimeSpan?>("RequestTimeout", str => TimeSpan.Parse(str));

        public TimeSpan? DefaultGetUpdatesTimeout => _configuration.Get<TimeSpan?>("DefaultGetUpdatesTimeout", str => TimeSpan.Parse(str));
    }
}