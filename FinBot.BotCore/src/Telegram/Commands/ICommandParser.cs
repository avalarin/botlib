using System.Threading.Tasks;
using FinBot.BotCore.Middlewares;
using FinBot.BotCore.Utils;

namespace FinBot.BotCore.Telegram.Commands {
    public interface ICommandParser {
        Task<CommandInfo> Parse(MiddlewareData data);
    }
}