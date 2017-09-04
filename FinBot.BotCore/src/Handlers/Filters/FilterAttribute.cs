using System;

namespace FinBot.BotCore.Handlers.Filters {
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public abstract class FilterAttribute: Attribute {
    }
}