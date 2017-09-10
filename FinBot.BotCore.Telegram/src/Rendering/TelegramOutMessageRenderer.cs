using System;
using System.Threading;
using System.Threading.Tasks;
using FinBot.BotCore.Context;
using FinBot.BotCore.Middlewares;
using FinBot.BotCore.Rendering;
using FinBot.BotCore.Telegram.Client;
using FinBot.BotCore.Telegram.Features;
using FinBot.BotCore.Telegram.Models;
using FinBot.BotCore.Utils;

namespace FinBot.BotCore.Telegram.Rendering {
    public class TelegramOutMessageRenderer : AbstractOutMessageRenderer<BaseOutMessage> {
        private readonly ITelegramClient _telegramClient;

        public TelegramOutMessageRenderer(ITelegramClient telegramClient) {
            _telegramClient = telegramClient;
        }

        public override async Task<MiddlewareData> RenderAsync(MiddlewareData middlewareData, BaseOutMessage message) {
            if (message.GetType() != typeof(BaseOutMessage) && message.GetType() != typeof(TelegramOutMessage)) {
                throw new ArgumentException("Unsupported message type " + message.GetType());
            }
            
            var updateFeature = middlewareData.Features.RequireOne<UpdateInfoFeature>();
            var update = updateFeature.Update;
            
            var chatId = GetChatId(middlewareData, message);

            var newMiddlewareData = middlewareData;

            if (message is TelegramOutMessage telegramOutMessage && telegramOutMessage.UpdateMessage) {
                var messageId = GetMessageId(middlewareData, message);
                await _telegramClient.UpdateMessage(new UpdateMessageData(
                    chatId, messageId, GetMessageContent(message)
                ));
            }
            else {
                var newMessage = await _telegramClient.SendMessage(new SendMessageData(
                    chatId, GetMessageContent(message)
                ));

                if (newMiddlewareData.Features.Has<MessageContextFeature>()) {
                    newMiddlewareData = newMiddlewareData.UpdateFeatures(f =>
                        f.ReplaceExclusive<MessageContextFeature>(context => context.PutMessageId(newMessage.Id.ToString()))
                    );
                }
            }

            if (update.CallbackQuery != null) {
                await _telegramClient.AnswerCallbackQuery(new AnswerCallbackQueryData(update.CallbackQuery.Id));
            }
            
            return newMiddlewareData;
        }

        private MessageContent GetMessageContent(BaseOutMessage outMessage) {
            if (outMessage is TelegramOutMessage telegramOutMessage) {
                return telegramOutMessage.MessageContent;
            }
            return new MessageContent() {
                Text = outMessage.Text
            };
        }
        
        private string GetChatId(MiddlewareData middlewareData, BaseOutMessage outMessage) {
            if (outMessage is TelegramOutMessage telegramOutMessage && telegramOutMessage.ChatId != null) {
                return telegramOutMessage.ChatId;
            }
            var message = middlewareData.Features.RequireOne<UpdateInfoFeature>().GetAnyMessage() ?? 
                                throw new InvalidOperationException("Cannot send response");
            return message.Chat.Id.ToString();
        }
        
        private long GetMessageId(MiddlewareData middlewareData, BaseOutMessage outMessage) {
            if (outMessage is TelegramOutMessage telegramOutMessage && telegramOutMessage.MessageId != null) {
                return telegramOutMessage.MessageId ?? 0;
            }
            var callbackQuery = middlewareData.Features.RequireOne<UpdateInfoFeature>().Update?.CallbackQuery ?? 
                          throw new InvalidOperationException("CallbackQuery is required for this renderer");
            return callbackQuery.Message.Id;
        }
    }
}