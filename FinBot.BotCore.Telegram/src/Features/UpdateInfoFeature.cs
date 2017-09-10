using System.Collections.Generic;
using System.Linq;
using FinBot.BotCore.Middlewares;
using FinBot.BotCore.ParameterMatching;
using FinBot.BotCore.Telegram.Models;
using FinBot.BotCore.Utils;

namespace FinBot.BotCore.Telegram.Features {
    public class UpdateInfoFeature : IFeature, IParameterValuesSource {
        public UpdateInfo Update { get; }

        public UpdateInfoFeature(UpdateInfo update) {
            Update = update;
        }

        public MessageInfo GetAnyMessage() {
            return Update.Message ?? Update.EditedMessage ?? Update.ChannelPost ?? Update.EditedChannelPost ?? Update.CallbackQuery?.Message;
        }

        public IEnumerable<ParameterValue> GetValues() {
            var message = GetAnyMessage();
            return (new List<KeyValuePair<string, object>>() {
                new KeyValuePair<string, object>("update", Update),
                new KeyValuePair<string, object>("message", message),
                new KeyValuePair<string, object>("messageText", message?.Text),
                new KeyValuePair<string, object>("callbackQuery", Update.CallbackQuery),
                new KeyValuePair<string, object>("callbackQueryData", Update.CallbackQuery?.Data),
                new KeyValuePair<string, object>("chat", message.Chat)
            }).Where(kvp => kvp.Value != null).Select(kvp => new ParameterValue(kvp.Key, kvp.Value));
        }
    }
}