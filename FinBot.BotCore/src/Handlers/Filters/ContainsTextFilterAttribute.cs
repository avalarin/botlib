using System.Threading.Tasks;
using FinBot.BotCore.Middlewares;
using FinBot.BotCore.Telegram.Features;

namespace FinBot.BotCore.Handlers.Filters {
    public class ContainsTextFilterAttribute : FilterAttribute, IFilter {
        public string Text { get; set; }

        public Task<FilterResult> FilterAsync(FilterAttribute attribute, MiddlewareData data) {
            var text = data.Features.RequireOne<UpdateInfoFeature>().GetAnyMessage().Text;
            if (text.Contains(Text)) {
                return Task.FromResult(FilterResult.NextFilter(data));
            }
            return Task.FromResult(FilterResult.SkipHandler());
        }
    }
}