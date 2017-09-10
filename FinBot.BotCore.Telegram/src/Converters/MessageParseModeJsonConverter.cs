using System;
using FinBot.BotCore.Telegram.Models;
using Newtonsoft.Json;

namespace FinBot.BotCore.Telegram.Converters {
    public class MessageParseModeJsonConverter : JsonConverter {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
            var mode = (MessageParseMode) value;
            switch (mode) {
                case MessageParseMode.HTML:
                    writer.WriteValue("HTML");
                    break;
                case MessageParseMode.Markdown:
                    writer.WriteValue("Markdown");
                    break;
                default:
                    throw new ArgumentException("Invalid parse mode value " + value);
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
            throw new NotSupportedException();
        }

        public override bool CanConvert(Type objectType) {
            return objectType == typeof(MessageParseMode);
        }
    }
}