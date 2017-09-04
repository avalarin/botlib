using FinBot.BotCore.Middlewares;
using FinBot.BotCore.Utils;

namespace FinBot.BotCore.Handlers.Filters {
    public class FilterResult {
        
        public bool Successful { get; }
        
        public Maybe<MiddlewareData> MiddlewareData { get; }

        private FilterResult(bool successful, MiddlewareData middlewareData) {
            Successful = successful;
            MiddlewareData = middlewareData.Nullable();
        }

        public static FilterResult CreateSuccessful(MiddlewareData middlewareData) {
            return new FilterResult(true, middlewareData);
        }
        
        public static FilterResult CreateUnsuccessful() {
            return new FilterResult(false, null);
        }
        
    }
}