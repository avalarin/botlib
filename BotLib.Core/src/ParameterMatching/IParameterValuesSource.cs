using System.Collections.Generic;

namespace BotLib.Core.ParameterMatching {
    public interface IParameterValuesSource {
        IEnumerable<ParameterValue> GetValues();
    }
}