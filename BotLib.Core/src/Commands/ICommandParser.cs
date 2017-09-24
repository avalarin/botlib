using System.Threading.Tasks;
using BotLib.Core.Middlewares;

namespace BotLib.Core.Commands {
    public interface ICommandParser {
        Task<CommandInfo> ParseAsync(MiddlewareData data);
    }
}