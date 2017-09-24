using System.Threading.Tasks;
using BotLib.Core.Middlewares;

namespace BotLib.Core.Handlers.Filters {
    public interface IFilter {
        Task<FilterResult> FilterAsync(FilterAttribute attribute, MiddlewareData data);
    }
}