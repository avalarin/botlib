using FinBot.BotCore.Middlewares;

namespace FinBot.BotCore.Telegram.Commands {
    public class CommandFeature : IFeature {

        public CommandInfo Command { get; }

        public CommandFeature(CommandInfo command) {
            Command = command;
        }

    }
}