using BotLib.Core.Middlewares;
using BotLib.Core.Utils;

namespace BotLib.Core.Handlers.Filters {
    public class FilterResult {
        
        public FilterAction Action { get; }
        
        public Maybe<MiddlewareData> MiddlewareData { get; }

        private FilterResult(FilterAction action, MiddlewareData middlewareData) {
            Action = action;
            MiddlewareData = middlewareData.Nullable();
        }

        public static FilterResult NextFilter(MiddlewareData middlewareData) {
            return new FilterResult(FilterAction.NextFilter, middlewareData);
        }
        
        public static FilterResult BreakExecution(MiddlewareData middlewareData) {
            return new FilterResult(FilterAction.BreakExecution, middlewareData);
        }
       
        public static FilterResult SkipHandler() {
            return new FilterResult(FilterAction.SkipHandler, null);
        }
        
    }

    public enum FilterAction {
        NextFilter,
        BreakExecution,
        SkipHandler
    }
}