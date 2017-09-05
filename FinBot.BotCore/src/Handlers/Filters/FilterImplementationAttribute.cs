using System;

namespace FinBot.BotCore.Handlers.Filters {
    [AttributeUsage(AttributeTargets.Class)]
    public class FilterImplementationAttribute : Attribute {
        public Type ImplementationType { get; }

        public FilterImplementationAttribute(Type implementationType) {
            ImplementationType = implementationType;
        }
    }
}