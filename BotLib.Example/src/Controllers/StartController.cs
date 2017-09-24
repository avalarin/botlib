using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BotLib.Core.Context;
using BotLib.Core.Handlers;
using BotLib.Core.ParameterMatching;
using BotLib.Core.Security;
using BotLib.Telegram.Commands;
using BotLib.Telegram.Handlers;
using BotLib.Telegram.Models;
using static BotLib.Telegram.Handlers.TelegramHandlerResultCreators;

namespace BotLib.Example.Controllers {
    public class StartController {
        private readonly IAuthenticationManager _authenticationManager;

        public StartController(IAuthenticationManager authenticationManager) {
            _authenticationManager = authenticationManager;
        }

        [Handler(Command = "start")]
        public IEnumerable<IHandlerResult> Start(MessageInfo message, MessageContext messageContext) {
            yield return new HandlerResultBuilder()
                .Text($"Привет {message.Chat.UserName}")
                .InlineKeyboard(b => b.Row(r => r.Button("a", "A").Button("b", "B"))
                                      .Row(r => r.Button("c", "C").Button("d", "D")))
                .Create();
            
            yield return HandlerResultCreators.PutToMessageContext("data", $"Текст от {message.Chat.UserName}: ");
        }
        
        [Handler(CommandPattern = "^[abcd]$", Type = TelegramCommandTypes.InlineKeyboardCommand)]
        public IEnumerable<IHandlerResult> InlineButton([StrictName] string callbackQueryData, MessageContext messageContext) {
            var newData = messageContext.Get<string>("data")
                .OrElseThrow(() => new InvalidOperationException("data is required"))
                + callbackQueryData;
            
            yield return new HandlerResultBuilder()
                .UpdateMessage()
                .Text(newData)
                .InlineKeyboard(b => b.Row(r => r.Button("a", "A").Button("b", "B"))
                                      .Row(r => r.Button("c", "C").Button("d", "D")))
                .Create();

            yield return HandlerResultCreators.PutToMessageContext("data", newData);
        }

        [Handler(Commands = new[] { "login", "signin" })]
        public async Task<IHandlerResult> Login(ApplicationUser applicationUser) {
            if (applicationUser.Is​Authenticated) {
                return Text("Уже авторизован");
            }

            await _authenticationManager.AuthenticateIdentity(applicationUser);
            
            return Text("Ты успешно авторизован, " + applicationUser.Id);
        }
        
        [Handler(Commands = new[] { "logout", "signout" })]
        public async Task<IHandlerResult> Logout(ApplicationUser applicationUser) {
            if (!applicationUser.Is​Authenticated) {
                return Text("Не авторизован");
            }
            
            await _authenticationManager.UnauthenticateIdentity(applicationUser);
            
            return Text("Ты успешно вышел, " + applicationUser.Id);
        }
        
        [Handler(Command = "test")]
        public IHandlerResult Test(ApplicationUser applicationUser) {
            if (applicationUser.Is​Authenticated) {
                return Text("Ты авторизовн, " + applicationUser.Id)
                    .Join(Text("Привет"));
            }
            return Text("Ты не авторизовн, " + applicationUser.Id)
                .Join(Text("Пока"));
        }
        
        [Handler(Command = "secured")]
        [Authorize]
        public IHandlerResult Test() {
            return Text("Только авторизованные попадают сюда");
        }
        
    }
}