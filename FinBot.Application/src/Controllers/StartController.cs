using FinBot.BotCore.Handlers;
using FinBot.BotCore.Handlers.Filters;
using FinBot.BotCore.ParameterMatching;
using FinBot.BotCore.Telegram.Models;
using static FinBot.BotCore.Handlers.HandlerResultCreators;

namespace FinBot.Application.Controllers {
    public class StartController {

        [Handler(Command = "start")]
        [ContainsTextFilter(Text = "abc")]
        public IHandlerResult Start(MessageInfo message) {
            return new HandlerResultBuilder()
                .Text($"Привет {message.Chat.UserName}")
                .InlineKeyboard(b => b.Row(r => r.Button("a", "A").Button("b", "B"))
                                      .Row(r => r.Button("c", "C").Button("d", "D")))
                .Create();
        }

        [Handler(Command = "test")]
        public IHandlerResult Test() {
            return Text("Привет").Join(Text("Пока"));
        }
        
        [Handler(InlineKeyboardButton = "a")]
        public IHandlerResult InlineButton([StrictName] string callbackQueryData) {
            return Text("OK " + callbackQueryData);
        }

    }
}