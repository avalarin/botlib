using System;

namespace BotLib.Core.Handlers.Filters {
    [AttributeUsage(AttributeTargets.Class)]
    public class FilterImplementationAttribute : Attribute {
        public Type ImplementationType { get; }

        public FilterImplementationAttribute(Type implementationType) {
            ImplementationType = implementationType;
        }
    }
}