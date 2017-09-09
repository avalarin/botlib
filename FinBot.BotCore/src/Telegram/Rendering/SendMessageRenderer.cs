using System;
using System.Threading;
using System.Threading.Tasks;
using FinBot.BotCore.Context;
using FinBot.BotCore.Middlewares;
using FinBot.BotCore.Telegram.Client;
using FinBot.BotCore.Telegram.Features;
using FinBot.BotCore.Telegram.Models;
using FinBot.BotCore.Utils;

namespace FinBot.BotCore.Telegram.Rendering {
    public class SendMessageRenderer : IClientRenderer {
        public MessageContent MessageContent { get; }
        
        public string ChatId { get; set; }
        
        public long? MessageId { get; set; }
        
        public bool UpdateMessage { get; set; }

        public SendMessageRenderer(MessageContent messageContent) {
            MessageContent = messageContent;
        }

        public async Task<MiddlewareData> Render(MiddlewareData middlewareData, ITelegramClient telegramClient, CancellationToken cancellationToken) {
            var updateFeature = middlewareData.Features.RequireOne<UpdateInfoFeature>();
            var update = updateFeature.Update;
            
            var chatId = ChatId ?? GetChatId(middlewareData);

            var newMiddlewareData = middlewareData;

            if (!UpdateMessage) {
                var newMessage = await telegramClient.SendMessage(new SendMessageData(
                    chatId, MessageContent
                ), cancellationToken);

                newMiddlewareData = newMiddlewareData.UpdateFeatures(f =>
                    f.ReplaceExclusive<MessageContextFeature>(context => context.PutMessageId(newMessage.Id))
                );
            }
            else {
                var messageId = MessageId ?? GetMessageId(middlewareData);
                await telegramClient.UpdateMessage(new UpdateMessageData(
                    chatId, messageId, MessageContent
                ), cancellationToken);
            }

            if (update.CallbackQuery != null) {
                await telegramClient.AnswerCallbackQuery(new AnswerCallbackQueryData(update.CallbackQuery.Id), cancellationToken);
            }
            
            return newMiddlewareData;
        }

        private string GetChatId(MiddlewareData middlewareData) {
            var message = middlewareData.Features.RequireOne<UpdateInfoFeature>().GetAnyMessage() ?? 
                                throw new InvalidOperationException("Cannot send response");
            return message.Chat.Id.ToString();
        }
        
        private long GetMessageId(MiddlewareData middlewareData) {
            var callbackQuery = middlewareData.Features.RequireOne<UpdateInfoFeature>().Update?.CallbackQuery ?? 
                          throw new InvalidOperationException("CallbackQuery is required for this renderer");
            return callbackQuery.Message.Id;
        }
        
    }
}