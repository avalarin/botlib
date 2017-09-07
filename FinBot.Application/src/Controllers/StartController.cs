using System.Net.Security;
using System.Threading.Tasks;
using FinBot.BotCore.Handlers;
using FinBot.BotCore.ParameterMatching;
using FinBot.BotCore.Security;
using FinBot.BotCore.Telegram.Models;
using static FinBot.BotCore.Handlers.HandlerResultCreators;

namespace FinBot.Application.Controllers {
    public class StartController {
        private readonly IAuthenticationManager _authenticationManager;

        public StartController(IAuthenticationManager authenticationManager) {
            _authenticationManager = authenticationManager;
        }

        [Handler(Command = "start")]
        public IHandlerResult Start(MessageInfo message, string token) {
            return new HandlerResultBuilder()
                .Text($"Привет {message.Chat.UserName}")
                .InlineKeyboard(b => b.Row(r => r.Button("a", "A").Button("b", "B"))
                                      .Row(r => r.Button("c", "C").Button("d", "D")))
                .Create();
        }

        [Handler(Command = "login")]
        public async Task<IHandlerResult> Login(ApplicationUser applicationUser) {
            if (applicationUser.Is​Authenticated) {
                return Text("Уже авторизован");
            }

            await _authenticationManager.AuthenticateIdentity(applicationUser);
            
            return Text("Ты успешно авторизован, " + applicationUser.Id);
        }
        
        [Handler(Command = "logout")]
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
        
        [Handler(InlineKeyboardButton = "a")]
        public IHandlerResult InlineButton([StrictName] string callbackQueryData) {
            return Text("OK " + callbackQueryData);
        }

    }
}