using System;

namespace FinBot.BotCore.Handlers {
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class HandlerAttribute : Attribute {
        public string Type { get; set; }
        public string Command { get; set; }
    }
}