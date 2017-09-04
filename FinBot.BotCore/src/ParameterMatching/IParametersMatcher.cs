using System.Collections.Generic;
using System.Reflection;
using FinBot.BotCore.Middlewares;
using FinBot.BotCore.Utils;

namespace FinBot.BotCore.ParameterMatching {
    public interface IParametersMatcher {
        Maybe<ParameterValue[]> MatchParameters(MiddlewareData data, IEnumerable<ParameterInfo> parameters);
    }
}