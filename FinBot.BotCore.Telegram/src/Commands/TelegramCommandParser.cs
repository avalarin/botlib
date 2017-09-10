using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FinBot.BotCore.Commands;
using FinBot.BotCore.Middlewares;
using FinBot.BotCore.Telegram.Features;
using FinBot.BotCore.Utils;

namespace FinBot.BotCore.Telegram.Commands {
    public class TelegramCommandParser : ICommandParser {
        private static readonly Regex ParseCommandRegex =
            new Regex(@"^\s*/([\w\d]+)(?:\s(.*))?$", RegexOptions.Compiled | RegexOptions.Multiline);
        
        private static readonly Regex ParseCallbackQueryRegex =
            new Regex(@"^\s*([\w\d]+)(?:\s(.*))?$", RegexOptions.Compiled | RegexOptions.Multiline);

        public Task<CommandInfo> ParseAsync(MiddlewareData data) {
            return Task.FromResult(Parse(data));
        }
        
        public CommandInfo Parse(MiddlewareData data) {
            var updateInfo = data.Features.RequireOne<UpdateInfoFeature>();

            if (updateInfo.Update.CallbackQuery != null) {
                return MatchCommand(TelegramCommandTypes.InlineKeyboardCommand, updateInfo.Update.CallbackQuery.Data, ParseCallbackQueryRegex);
            }
            return MatchCommand(TelegramCommandTypes.TextMessageCommand, updateInfo.GetAnyMessage().Text, ParseCommandRegex);
        }

        private static CommandInfo MatchCommand(string type, string text, Regex regex) {
            return regex.Match(text)
                .NotNull()
                .Filter(m => m.Success)
                .Map(m => CommandInfo.WithCommand(type, m.Groups[1].Value, m.Groups[2].Nullable().Map(g => g.Value).OrElse(""), text))
                .OrElseGet(() => CommandInfo.WithoutCommand(type, text));
        }
    }
}