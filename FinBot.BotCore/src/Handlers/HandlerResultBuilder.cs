using System;
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

        public HandlerResultBuilder Text(string text) {
            _text = text;
            return this;
        }

        public HandlerResultBuilder InlineKeyboard(Func<InlineKeyboardMarkup.InlineKeyboardMarkupBuilder, InlineKeyboardMarkup.InlineKeyboardMarkupBuilder> factory) {
            _inlineKeyboardMarkup = factory(InlineKeyboardMarkup.Builder()).Build();
            return this;
        }
            
        public IHandlerResult Create() {
            return new HandlerResult() {
                Text = _text,
                InlineKeyboardMarkup = _inlineKeyboardMarkup
            };
        } 
        
        private class HandlerResult : IHandlerResult {
            public string Text { get; set; }
        
            public InlineKeyboardMarkup InlineKeyboardMarkup { get; set; }

            public Task<MiddlewareData> RenderAsync(MiddlewareData data) {
                var messageContent = new MessageContent(Text);

                InlineKeyboardMarkup.Nullable().IfPresent(m => messageContent.ReplyMarkup = m);

                var renderer = new ResponseMessageRenderer(messageContent);
                var newData = data.UpdateFeatures(f => f.Add<ClientRendererFeature>(new ClientRendererFeature(renderer)));
                return Task.FromResult(newData);
            }
        
        }
        
    }
}