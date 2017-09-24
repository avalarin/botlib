using BotLib.Core.Middlewares;

namespace BotLib.Core.Commands {
    public class CommandFeature : IFeature {

        public CommandInfo Command { get; }

        public CommandFeature(CommandInfo command) {
            Command = command;
        }

    }
}