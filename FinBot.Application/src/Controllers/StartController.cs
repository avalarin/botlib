using FinBot.BotCore.Handlers;
using FinBot.BotCore.Handlers.Filters;
using FinBot.BotCore.ParameterMatching;
using FinBot.BotCore.Telegram.Models;

namespace FinBot.Application.Controllers {
    public class StartController {

        [Handler(Command = "start")]
        [ContainsTextFilter(Text = "abc")]
        public IHandlerResult Start(MessageInfo message) {
            return HandlerResult.Builder()
                .Text($"Привет {message.Chat.UserName}")
                .InlineKeyboard(b => b.Row(r => r.Button("a", "A").Button("b", "B"))
                                      .Row(r => r.Button("c", "C").Button("d", "D")))
                .Create();
        }

        [Handler(InlineKeyboardButton = "a")]
        [Handler(InlineKeyboardButton = "b")]
        [Handler(InlineKeyboardButton = "c")]
        [Handler(InlineKeyboardButton = "d")]
        public IHandlerResult InlineButton([StrictName] string callbackQueryData) {
            return HandlerResult.WithText("OK " + callbackQueryData);
        }

    }
}