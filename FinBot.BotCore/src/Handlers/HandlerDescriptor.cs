using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using FinBot.BotCore.Handlers.Filters;
using FinBot.BotCore.Middlewares;
using FinBot.BotCore.ParameterMatching;
using FinBot.BotCore.Telegram.Commands;
using FinBot.BotCore.Telegram.Features;
using FinBot.BotCore.Telegram.Models;
using FinBot.BotCore.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace FinBot.BotCore.Handlers {
    public class HandlerDescriptor {
        public MethodInfo Method { get; }

        private Maybe<string> Command { get; }
        
        private Maybe<string> InlineKeyboardButton { get; }

        private HandlerDescriptor(MethodInfo method, string command, string inlineKeyboardButton) {
            Method = method;
            Command = command.Nullable();
            InlineKeyboardButton = inlineKeyboardButton.Nullable();
        }
        
        public HandlerMatch Match(MiddlewareData middlewareData, IServiceProvider serviceProvider) {
            var callbackQuery = middlewareData.Features.RequireOne<UpdateInfoFeature>().Update.CallbackQuery.Nullable();
            var command = middlewareData.Features.RequireOne<CommandFeature>().Command;

            var commandMatched = Command
                .Map(a => command.Command.Map(b => a.Equals(b, StringComparison.OrdinalIgnoreCase)).OrElse(false))
                .OrElse(false);

            var ikbMatched = InlineKeyboardButton
                .Map(a => callbackQuery.Map(b => a.Equals(b.Data, StringComparison.OrdinalIgnoreCase)).OrElse(false))
                .OrElse(false);

//            var filterResult = FilterUtils.ExecuteFilters(Filters, serviceProvider, middlewareData).Result; // TODO async
//            if (!filterResult.Successful) {
//                return HandlerMatch.CreateUnmatched();
//            }
//            // TODO save middleware data
            
            if (commandMatched || ikbMatched) {
                return serviceProvider.GetService<IParametersMatcher>().MatchParameters(middlewareData, Method.GetParameters())
                    .Map(values => HandlerMatch.CreateMatched(this, values))
                    .OrElseGet(HandlerMatch.CreateUnmatched);
            }

            return HandlerMatch.CreateUnmatched();
        }

        public static HandlerDescriptor Create(MethodInfo method, HandlerAttribute attribute) {
            return new HandlerDescriptor(method, attribute.Command, attribute.InlineKeyboardButton);
        }

        public class HandlerMatch {
        
            public bool Successful { get; }
            
            public HandlerDescriptor Descriptor { get; }

            public ParameterValue[] Values { get; }

            private HandlerMatch(bool successful, HandlerDescriptor descriptor, ParameterValue[] values) {
                Successful = successful;
                Descriptor = descriptor;
                Values = values;
            }

            public static HandlerMatch CreateMatched(HandlerDescriptor descriptor, ParameterValue[] values) {
                return new HandlerMatch(true, descriptor, values);
            }
            
            public static HandlerMatch CreateUnmatched() {
                return new HandlerMatch(false, null, null);
            }
        }


    }
}