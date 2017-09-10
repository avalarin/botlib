using System;
using System.Reflection;
using FinBot.BotCore.Commands;
using FinBot.BotCore.Middlewares;
using FinBot.BotCore.Utils;

namespace FinBot.BotCore.Handlers {
    public class HandlerDescriptor {
        public MethodInfo Method { get; }

        private Maybe<string> Command { get; }
        
        private Maybe<string> Type { get; }

        private HandlerDescriptor(MethodInfo method, string command, string type) {
            Method = method;
            Command = command.Nullable();
            Type = type.Nullable();
        }
        
        public HandlerMatch Match(MiddlewareData middlewareData) {
            var command = middlewareData.Features.RequireOne<CommandFeature>().Command;

            var commandMatched = Command
                .Map(a => command.Command.Map(b => a.Equals(b, StringComparison.OrdinalIgnoreCase)).OrElse(false))
                .OrElse(false);

            var typeMatched = Type
                .Map(filter => filter.Equals(command.Type, StringComparison.Ordinal))
                .OrElse(true);

            if (commandMatched && typeMatched) {
                return HandlerMatch.CreateMatched(this);
            }

            return HandlerMatch.CreateUnmatched();
        }

        public static HandlerDescriptor Create(MethodInfo method, HandlerAttribute attribute) {
            return new HandlerDescriptor(method, attribute.Command, attribute.Type);
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