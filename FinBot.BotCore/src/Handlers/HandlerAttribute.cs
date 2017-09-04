using System;

namespace FinBot.BotCore.Handlers {
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class HandlerAttribute : Attribute {
        public string Command { get; set; }
        
        public string InlineKeyboardButton { get; set; }
    }
}