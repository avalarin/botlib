using System;

namespace BotLib.Telegram.Polling {
    public interface IAutoPollerConfiguration {
        bool Enabled { get; }
        int? OneTimeLimit { get; }
        TimeSpan? PoolingTimeout { get; }
        string[] FieldsFilter { get; }
    }
}