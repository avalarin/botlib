using System;

namespace FinBot.BotCore.Exceptions {
    public class UnauthorizedException : Exception {
        public UnauthorizedException()
            : base("Unauthorized") {
        }

        public UnauthorizedException(Exception inner)
            : base("Unauthorized", inner) {
        }
    }
}