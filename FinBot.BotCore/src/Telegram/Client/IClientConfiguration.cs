using System;

namespace FinBot.BotCore.Telegram.Client {
    public interface IClientConfiguration {
        string Token { get; }
        TimeSpan? RequestTimeout { get; }
        TimeSpan? DefaultGetUpdatesTimeout { get; }
    }
}