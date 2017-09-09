using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinBot.BotCore.Middlewares;
using FinBot.BotCore.Telegram.Features;

namespace FinBot.BotCore.Context {
    public class ContextMiddleware : IMiddleware {
        private readonly IContextStorage _storage;

        public ContextMiddleware(IContextStorage storage) {
            _storage = storage;
        }

        public async Task<MiddlewareData> InvokeAsync(MiddlewareData data, IMiddlewaresChain chain) {
            var updateInfo = data.Features.RequireOne<UpdateInfoFeature>();
            var message = updateInfo.GetAnyMessage();
            var chatContext = await _storage.LoadChatContext(message.Chat.Id);
            var messageContext = Enumerable.Empty<KeyValuePair<string, object>>(); 
                       
            var contextMessageId = updateInfo.Update.EditedMessage?.Id
                                ?? updateInfo.Update.EditedChannelPost?.Id
                                ?? updateInfo.Update.CallbackQuery?.Message.Id;
            if (contextMessageId != null) {
                messageContext = await _storage.LoadMessageContext(message.Chat.Id, contextMessageId.Value);
            }
            
            var newData = data.UpdateFeatures(f => f.AddExclusive<ChatContextFeature>(new ChatContextFeature(chatContext))
                                                    .AddExclusive<MessageContextFeature>(new MessageContextFeature(message.Chat.Id.ToString(), message.Id, messageContext)));
            var resultData = await chain.NextAsync(newData);

            var newChatContext = resultData.Features.RequireOne<ChatContextFeature>();
            await _storage.SaveChatContext(message.Chat.Id, newChatContext.Items);

            var newMessageContext = resultData.Features.RequireOne<MessageContextFeature>();
            foreach (var messageId in newMessageContext.MessageIds) {
                await _storage.SaveMessageContext(message.Chat.Id, messageId, newMessageContext.Items);
            }
            
            return resultData;
        }
    }
}