using System;
using System.Threading;
using System.Threading.Tasks;
using FinBot.BotCore.Middlewares;
using FinBot.BotCore.Telegram.Client;
using FinBot.BotCore.Telegram.Features;
using FinBot.BotCore.Telegram.Models;

namespace FinBot.BotCore.Telegram.Rendering {
    public class ResponseMessageRenderer : IClientRenderer {
        private readonly MessageContent _messageContent;

        public ResponseMessageRenderer(MessageContent messageContent) {
            _messageContent = messageContent;
        }

        public async Task<MiddlewareData> Render(MiddlewareData middlewareData, ITelegramClient telegramClient, CancellationToken cancellationToken) {
            var updateFeature = middlewareData.Features.RequireOne<UpdateInfoFeature>();
            var update = updateFeature.Update;
            var message = updateFeature.GetAnyMessage();

            if (message == null) {
                throw new InvalidOperationException("Cannot send response");
            }

//            if (context.SentMessageIds.Any()) {
//                await telegramClient.UpdateMessage(new UpdateMessageData(
//                    message.Chat.Id.ToString(),
//                    context.SentMessageIds.First(),
//                    _messageData
//                ), cancellationToken);
//                return MessageContext.Empty;
//            }

            var chatId = message.Chat.Id.ToString();
            var newMessage = await telegramClient.SendMessage(new SendMessageData(
                chatId, _messageContent
            ), cancellationToken);

            if (update.CallbackQuery != null) {
                await telegramClient.AnswerCallbackQuery(new AnswerCallbackQueryData(update.CallbackQuery.Id), cancellationToken);
            }
            
            return middlewareData;
        }
    }
}