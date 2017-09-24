using System.Collections.Generic;
using System.Reflection;
using BotLib.Core.Middlewares;
using BotLib.Core.Utils;

namespace BotLib.Core.ParameterMatching {
    public interface IParametersMatcher {
        Maybe<ParameterValue[]> MatchParameters(MiddlewareData data, IEnumerable<ParameterInfo> parameters);
    }
}