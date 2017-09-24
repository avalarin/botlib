using System;

namespace BotLib.Core.Exceptions {
    public class UnauthorizedException : Exception {
        public UnauthorizedException()
            : base("Unauthorized") {
        }

        public UnauthorizedException(Exception inner)
            : base("Unauthorized", inner) {
        }
    }
}