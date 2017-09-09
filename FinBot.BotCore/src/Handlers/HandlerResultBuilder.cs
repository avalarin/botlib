﻿using System;
using System.Threading.Tasks;
using FinBot.BotCore.Middlewares;
using FinBot.BotCore.Telegram.Features;
using FinBot.BotCore.Telegram.Models;
using FinBot.BotCore.Telegram.Rendering;
using FinBot.BotCore.Utils;

namespace FinBot.BotCore.Handlers {
    public class HandlerResultBuilder {

        private string _text;
        private InlineKeyboardMarkup _inlineKeyboardMarkup;
        private bool _update;
        private long? _messageId;
        private string _chatId;

        public HandlerResultBuilder UpdateMessage() {
            _update = true;
            return this;
        }
        
        public HandlerResultBuilder Text(string text) {
            _text = text;
            return this;
        }

        public HandlerResultBuilder MessageId(long messageId) {
            _messageId = messageId;
            return this;
        }
        
        public HandlerResultBuilder ChatId(string chatId) {
            _chatId = chatId;
            return this;
        }
        
        public HandlerResultBuilder InlineKeyboard(Func<InlineKeyboardMarkup.InlineKeyboardMarkupBuilder, InlineKeyboardMarkup.InlineKeyboardMarkupBuilder> factory) {
            _inlineKeyboardMarkup = factory(InlineKeyboardMarkup.Builder()).Build();
            return this;
        }
            
        public IHandlerResult Create() {
            return new HandlerResult() {
                Text = _text,
                InlineKeyboardMarkup = _inlineKeyboardMarkup,
                Update = _update,
                MessageId = _messageId,
                ChatId = _chatId
            };
        } 
        
        private class HandlerResult : IHandlerResult {
            public string Text { get; set; }
        
            public InlineKeyboardMarkup InlineKeyboardMarkup { get; set; }
            
            public bool Update { get; set; }
            
            public long? MessageId { get; set; }
            
            public string ChatId { get; set; }

            public Task<MiddlewareData> RenderAsync(MiddlewareData data) {
                var messageContent = new MessageContent(Text);

                InlineKeyboardMarkup.Nullable().IfPresent(m => messageContent.ReplyMarkup = m);

                var renderer = new SendMessageRenderer(messageContent) {
                    UpdateMessage = Update,
                    MessageId = MessageId,
                    ChatId = ChatId
                };
                var newData = data.UpdateFeatures(f => f.Add<ClientRendererFeature>(new ClientRendererFeature(renderer)));
                return Task.FromResult(newData);
            }
        
        }
        
    }
}