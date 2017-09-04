using FinBot.BotCore.Utils;

namespace FinBot.BotCore.Telegram.Commands {
    public class CommandInfo {

        public Maybe<string> Command { get; }
        
        public Maybe<string> Argument { get; }

        public string Content { get; }

        private CommandInfo(string command, string argument, string content) {
            Command = Maybe<string>.OfNullable(command);
            Argument = Maybe<string>.OfNullable(argument);
            Content = content;
        }

        public static CommandInfo WithoutCommand(string content) {
            return new CommandInfo(null, null, content);
        }
        
        public static CommandInfo WithCommand(string command, string argument, string content) {
            return new CommandInfo(command, argument, content);
        }
        
    }
}