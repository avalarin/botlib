using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace BotLib.Telegram.Models {
    [JsonObject(MemberSerialization.OptIn)]
    public class InlineKeyboardMarkup  {

        [JsonProperty(PropertyName = "inline_keyboard")]
        public IEnumerable<IEnumerable<InlineKeyboardButton>> InlineKeyboard { get; }

        public InlineKeyboardMarkup(IEnumerable<IEnumerable<InlineKeyboardButton>> inlineKeyboard) {
            InlineKeyboard = inlineKeyboard;
        }

        public static InlineKeyboardMarkupBuilder Builder() {
            return new InlineKeyboardMarkupBuilder();
        }

        public class InlineKeyboardMarkupBuilder {
            private readonly List<IEnumerable<InlineKeyboardButton>> _list = new List<IEnumerable<InlineKeyboardButton>>();

            public InlineKeyboardMarkupBuilder Row(Action<RowBuilder> action) {
                var rowBuilder = new RowBuilder();
                action(rowBuilder);
                _list.Add(rowBuilder.Build());
                return this;
            }

            public InlineKeyboardMarkup Build() {
                return new InlineKeyboardMarkup(_list.AsReadOnly());
            }

            public class RowBuilder {

                private readonly List<InlineKeyboardButton> _items = new List<InlineKeyboardButton>();

                public RowBuilder Button(string key, string text) {
                    _items.Add(new InlineKeyboardButton(text) { CallbackData = key} );
                    return this;
                }
                
                public RowBuilder Link(string link, string text) {
                    _items.Add(new InlineKeyboardButton(text) { Url = link} );
                    return this;
                }

                public IEnumerable<InlineKeyboardButton> Build() {
                    return _items;
                }

            }
        }

    }
}