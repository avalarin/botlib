using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using FinBot.BotCore.Middlewares;
using FinBot.BotCore.Utils;

namespace FinBot.BotCore.Handlers.Filters {
    public static class FilterUtils {
        public static async Task<FilterResult> ExecuteFilters(IEnumerable<FilterAttribute> filtets, IServiceProvider serviceProvider, MiddlewareData data) {
            var result = FilterResult.CreateSuccessful(data);
            foreach (var filter in filtets) {
                result = await ExecuteFilter(serviceProvider, filter, result.MiddlewareData.Value);
                if (!result.Successful) return result;
            }
            return result;
        }

        private static Task<FilterResult> ExecuteFilter(IServiceProvider serviceProvider, FilterAttribute attribute, MiddlewareData data) {
            if (attribute is IFilter filter) {
                return filter.FilterAsync(attribute, data);
            }

            var filterImplementationAttribute = attribute.GetType().GetTypeInfo().GetCustomAttribute<FilterImplementationAttribute>();
            if (filterImplementationAttribute != null) {
                var instance = (IFilter)serviceProvider.GetInstance(filterImplementationAttribute.ImplementationType);
                return instance.FilterAsync(attribute, data);
            }

            throw new InvalidOperationException("Cannot invoke filter " + attribute.GetType());
        }
    }
}