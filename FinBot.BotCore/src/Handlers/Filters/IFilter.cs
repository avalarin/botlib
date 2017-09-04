using System.Threading.Tasks;
using FinBot.BotCore.Middlewares;

namespace FinBot.BotCore.Handlers.Filters {
    public interface IFilter {
        Task<FilterResult> FilterAsync(FilterAttribute attribute, MiddlewareData data);
    }
}