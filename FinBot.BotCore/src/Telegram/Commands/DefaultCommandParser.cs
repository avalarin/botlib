using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FinBot.BotCore.Middlewares;
using FinBot.BotCore.Telegram.Features;
using FinBot.BotCore.Utils;

namespace FinBot.BotCore.Telegram.Commands {
    public class DefaultCommandParser : ICommandParser {
        private static readonly Regex ParseCommandRegex =
            new Regex(@"^\s*/([\w\d]+)(?:\s(.*))?$", RegexOptions.Compiled | RegexOptions.Multiline);

        public Task<CommandInfo> Parse(MiddlewareData data) {
            var message = data.Features.RequireOne<UpdateInfoFeature>().GetAnyMessage();
            var result =  ParseCommandRegex.Match(message.Text)
                .NotNull()
                .Filter(m => m.Success)
                .Map(m => CommandInfo.WithCommand(m.Groups[1].Value, m.Groups[2].Nullable().Map(g => g.Value).OrElse(""), message.Text))
                .OrElseGet(() => CommandInfo.WithoutCommand(message.Text));
            return Task.FromResult(result);
        }
    }
}