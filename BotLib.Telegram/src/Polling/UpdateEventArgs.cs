using System;
using BotLib.Telegram.Models;

namespace BotLib.Telegram.Polling {
    public class UpdateEventArgs : EventArgs {
        public UpdateInfo Update { get; }

        public UpdateEventArgs(UpdateInfo update) {
            Update = update;
        }
    }
}