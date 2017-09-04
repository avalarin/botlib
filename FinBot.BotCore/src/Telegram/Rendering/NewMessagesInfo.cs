using System.Collections.Generic;

namespace FinBot.BotCore.Telegram.Rendering {
    public class NewMessagesInfo {

        public IEnumerable<long> SentMessageIds { get; }

        public NewMessagesInfo(IEnumerable<long> sentMessageIds) {
            SentMessageIds = sentMessageIds;
        }

    }
}