using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using FinBot.BotCore.Commands;
using FinBot.BotCore.Middlewares;
using FinBot.BotCore.Utils;

namespace FinBot.BotCore.Handlers {
    public class HandlerDescriptor {
        public MethodInfo Method { get; }

        private HandlerAttribute Attribute { get; }
           
        private HandlerDescriptor(MethodInfo method, HandlerAttribute attribute) {
            Method = method;
            Attribute = attribute;
        }
        
        public HandlerMatch Match(MiddlewareData middlewareData) {
            var command = middlewareData.Features.RequireOne<CommandFeature>().Command;

            var commandMatched = Attribute.Command.Nullable()
                .Map(a => command.Command.Map(b => a.Equals(b, StringComparison.OrdinalIgnoreCase)).OrElse(false))
                .OrElse(false);
            
            var commandsMatched = Attribute.Commands.Nullable()
                .Map(a => command.Command.Map(b => a.Contains(b, StringComparer.OrdinalIgnoreCase)).OrElse(false))
                .OrElse(false);
            
            var commandPatternMatched = Attribute.CommandPattern.Nullable()
                .Map(pattern => new Regex(pattern, RegexOptions.Compiled))
                .Map(pattern => command.Command.Map(pattern.IsMatch).OrElse(false))
                .OrElse(false);

            var typeMatched = Attribute.Type.Nullable()
                .Map(filter => filter.Equals(command.Type, StringComparison.Ordinal))
                .OrElse(true);

            if (typeMatched && (commandMatched || commandsMatched || commandPatternMatched)) {
                return HandlerMatch.CreateMatched(this);
            }

            return HandlerMatch.CreateUnmatched();
        }

        public static HandlerDescriptor Create(MethodInfo method, HandlerAttribute attribute) {
            return new HandlerDescriptor(method, attribute);
        }

        public class HandlerMatch {
        
            public bool Successful { get; }
            
            public HandlerDescriptor Descriptor { get; }

            private HandlerMatch(bool successful, HandlerDescriptor descriptor) {
                Successful = successful;
                Descriptor = descriptor;
            }

            public static HandlerMatch CreateMatched(HandlerDescriptor descriptor) {
                return new HandlerMatch(true, descriptor);
            }
            
            public static HandlerMatch CreateUnmatched() {
                return new HandlerMatch(false, null);
            }
        }


    }
}