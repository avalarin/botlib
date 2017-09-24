using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BotLib.Core.Middlewares;
using BotLib.Core.Utils;

namespace BotLib.Core.ParameterMatching {
    public class DefaultParametersMatcher : IParametersMatcher {
        
        public Maybe<ParameterValue[]> MatchParameters(MiddlewareData data, IEnumerable<ParameterInfo> parameters) {
            var allValues = data.Features.GetAllOfBaseType<IParameterValuesSource>()
                .SelectMany(source => source.GetValues())
                .Union(new [] { new ParameterValue("middlewareData", data) });

            var groupped = parameters.GroupJoin(
                inner: allValues,
                outerKeySelector: p => p.ParameterType,
                innerKeySelector: v => v.Type,
                resultSelector: (parameter, values) => new {parameter, values = values.ToArray()},
                comparer: ParametersEqualityComparer.Instance
            );

            return groupped.Select(g => SelectValue(g.parameter, g.values))
                .NotNull()
                .Filter(values => values.All(v => v != null))
                .Map(values => values.ToArray());
        }

        private static ParameterValue SelectValue(ParameterInfo parameter, ParameterValue[] values) {
            if (values.Length > 1 || parameter.GetCustomAttribute<StrictNameAttribute>() != null) {
                try {
                    return values.SingleOrDefault(v => v.Name.Equals(parameter.Name));
                }
                catch (InvalidOperationException) {
                    throw new InvalidOperationException($"Cannot select value for parameter {parameter.ParameterType.FullName} {parameter.Name}");
                }
            }

            if (values.Length == 1) return values[0];
            return null;
        }

        private class ParametersEqualityComparer : IEqualityComparer<Type> {
            public static readonly ParametersEqualityComparer Instance = new ParametersEqualityComparer();

            public bool Equals(Type parameterType, Type valueType) {
                if (parameterType.IsMathesWithGenericDefinition(typeof(Nullable<>))) {
                    var typeOfItem = parameterType.GetTypeInfo().GetGenericArguments()[0];
                    return typeOfItem == valueType;
                }
                return parameterType == valueType;
            }

            public int GetHashCode(Type type) {
                return type.GetHashCode();
            }
        }
    }
}