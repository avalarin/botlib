﻿using System.Collections.Generic;
using BotLib.Core.Middlewares;
using BotLib.Core.ParameterMatching;
using BotLib.Telegram.Models;

namespace BotLib.Telegram.Features {
    public class UpdateInfoFeature : IFeature, IParameterValuesSource {
        public UpdateInfo Update { get; }

        public UpdateInfoFeature(UpdateInfo update) {
            Update = update;
        }

        public MessageInfo GetAnyMessage() {
            return Update.Message ?? Update.EditedMessage ?? Update.ChannelPost ?? Update.EditedChannelPost ?? Update.CallbackQuery?.Message;
        }

        public IEnumerable<ParameterValue> GetValues() {
            yield return new ParameterValue("update", Update);
            
            var message = GetAnyMessage();
            if (message != null) {
                yield return new ParameterValue("message", message);
                yield return new ParameterValue("messageText", message.Text);
                yield return new ParameterValue("chat", message.Chat);
            }

            var callbackQuery = Update.CallbackQuery;
            if (callbackQuery != null) {
                yield return new ParameterValue("callbackQuery", callbackQuery);
                yield return new ParameterValue("callbackQueryData", callbackQuery.Data);
            }
        }
    }
}