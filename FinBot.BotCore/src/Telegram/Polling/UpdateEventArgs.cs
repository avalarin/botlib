using System;
using FinBot.BotCore.Telegram.Models;

namespace FinBot.BotCore.Telegram.Polling {
    public class UpdateEventArgs : EventArgs {
        public UpdateInfo Update { get; }

        public UpdateEventArgs(UpdateInfo update) {
            Update = update;
        }
    }
}