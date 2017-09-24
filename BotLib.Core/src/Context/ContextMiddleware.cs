using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BotLib.Core.Middlewares;

namespace BotLib.Core.Context {
    public class ContextMiddleware : IMiddleware {
        private readonly IContextStorage _storage;
        private readonly IContextManager _contextManager;

        public ContextMiddleware(IContextStorage storage, IContextManager contextManager) {
            _storage = storage;
            _contextManager = contextManager;
        }

        public async Task<MiddlewareData> InvokeAsync(MiddlewareData data, IMiddlewaresChain chain) {
            var chatId = _contextManager.GetChatId(data);
            var messageId = _contextManager.GetMessageId(data);
            
            var chatContext = await _storage.LoadChatContext(chatId);
            var messageContext = Enumerable.Empty<KeyValuePair<string, object>>(); 

            if (_contextManager.HasMessageContext(data)) {
                messageContext = await _storage.LoadMessageContext(chatId, messageId);
            }
            
            var newData = data.UpdateFeatures(f => f.AddExclusive<ChatContextFeature>(new ChatContextFeature(chatContext))
                                                    .AddExclusive<MessageContextFeature>(new MessageContextFeature(messageId, messageContext)));
            var resultData = await chain.NextAsync(newData);

            var newChatContext = resultData.Features.RequireOne<ChatContextFeature>();
            await _storage.SaveChatContext(chatId, newChatContext.Items);

            var newMessageContext = resultData.Features.RequireOne<MessageContextFeature>();
            foreach (var id in newMessageContext.MessageIds) {
                await _storage.SaveMessageContext(chatId, id, newMessageContext.Items);
            }
            
            return resultData;
        }
    }
}