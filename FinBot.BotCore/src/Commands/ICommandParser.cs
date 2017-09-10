using System.Threading.Tasks;
using FinBot.BotCore.Middlewares;

namespace FinBot.BotCore.Commands {
    public interface ICommandParser {
        Task<CommandInfo> ParseAsync(MiddlewareData data);
    }
}