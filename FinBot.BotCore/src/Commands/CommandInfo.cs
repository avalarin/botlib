using FinBot.BotCore.Utils;

namespace FinBot.BotCore.Commands {
    public class CommandInfo {

        public string Type { get; }
        
        public Maybe<string> Command { get; }
        
        public Maybe<string> Argument { get; }

        public string Content { get; }

        private CommandInfo(string type, string command, string argument, string content) {
            Type = type;
            Command = Maybe<string>.OfNullable(command);
            Argument = Maybe<string>.OfNullable(argument);
            Content = content;
        }

        public static CommandInfo WithoutCommand(string type, string content) {
            return new CommandInfo(type, null, null, content);
        }
        
        public static CommandInfo WithCommand(string type, string command, string argument, string content) {
            return new CommandInfo(type, command, argument, content);
        }
        
    }
}