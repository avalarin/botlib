using System.Threading;
using System.Threading.Tasks;
using FinBot.BotCore.Middlewares;
using FinBot.BotCore.Telegram.Client;

namespace FinBot.BotCore.Telegram.Rendering {
    public interface IClientRenderer {
        Task<MiddlewareData> Render(MiddlewareData middlewareData, ITelegramClient telegramClient, CancellationToken cancellationToken);
    }
}