using System;

namespace BotLib.Core.Handlers.Filters {
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public abstract class FilterAttribute: Attribute {
    }
}