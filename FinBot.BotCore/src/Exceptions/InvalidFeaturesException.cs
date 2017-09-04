using System;

namespace FinBot.BotCore.Exceptions {
    public class InvalidFeaturesException : Exception {
        public InvalidFeaturesException(string message)
            : base(message) {
        }

        public InvalidFeaturesException(string message, Exception inner)
            : base(message, inner) {
        }
    }
}