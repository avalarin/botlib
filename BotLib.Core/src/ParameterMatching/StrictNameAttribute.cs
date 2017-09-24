using System;

namespace BotLib.Core.ParameterMatching {
    [AttributeUsage(AttributeTargets.Parameter)]
    public class StrictNameAttribute : Attribute {
    }
}