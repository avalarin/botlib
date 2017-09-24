using System;

namespace BotLib.Telegram.Client {
    public interface IClientConfiguration {
        string Token { get; }
        TimeSpan? RequestTimeout { get; }
        TimeSpan? DefaultGetUpdatesTimeout { get; }
    }
}