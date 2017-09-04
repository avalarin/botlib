using System;

namespace FinBot.BotCore.Exceptions {
    public class NoSuchHandlerException : Exception {
        public NoSuchHandlerException()
            : base("No such handler") {
        }

        public NoSuchHandlerException(Exception inner)
            : base("No such handler", inner) {
        }
    }
}