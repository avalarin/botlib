using Newtonsoft.Json;

namespace FinBot.BotCore.Telegram.Models {
    public class AnswerCallbackQueryData {
        
        [JsonProperty(PropertyName = "callback_query_id")]
        public string CallbackQueryId { get; }
        
        [JsonProperty(PropertyName = "text", NullValueHandling = NullValueHandling.Ignore)]
        public string Text { get; set; }
        
        [JsonProperty(PropertyName = "show_alert")]
        public bool ShowAlert { get; set; }
        
        [JsonProperty(PropertyName = "url", NullValueHandling = NullValueHandling.Ignore)]
        public string Url { get; set; }
        
        [JsonProperty(PropertyName = "cache_time")]
        public int CacheTime { get; set; }

        public AnswerCallbackQueryData(string callbackQueryId) {
            CallbackQueryId = callbackQueryId;
        }
    }
}