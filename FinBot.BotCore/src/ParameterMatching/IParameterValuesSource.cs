using System.Collections.Generic;

namespace FinBot.BotCore.ParameterMatching {
    public interface IParameterValuesSource {
        IEnumerable<ParameterValue> GetValues();
    }
}