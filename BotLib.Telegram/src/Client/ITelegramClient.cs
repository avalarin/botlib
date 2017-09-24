using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BotLib.Telegram.Models;

namespace BotLib.Telegram.Client {
    public interface ITelegramClient {
        Task<UserInfo> GetMe(CancellationToken cancellationToken = default(CancellationToken));

        Task<ChatInfo> GetChat(string chatId,
            CancellationToken cancellationToken = default(CancellationToken));

        Task<MessageInfo> SendMessage(SendMessageData data,
            CancellationToken cancellationToken = default(CancellationToken));

        Task<MessageInfo> UpdateMessage(UpdateMessageData data,
            CancellationToken cancellationToken = default(CancellationToken));
        
        Task<bool> AnswerCallbackQuery(AnswerCallbackQueryData data,
            CancellationToken cancellationToken = default(CancellationToken));

        Task<IEnumerable<UpdateInfo>> GetUpdates(
            long offset = 0,
            int limit = 100,
            TimeSpan? timeout = null,
            string[] filter = null,
            CancellationToken cancellationToken = default(CancellationToken));
    }
}