using System;

namespace FinBot.BotCore.ParameterMatching {
    [AttributeUsage(AttributeTargets.Parameter)]
    public class StrictNameAttribute : Attribute {
    }
}