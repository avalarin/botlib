using System;
using System.Threading.Tasks;
using FinBot.BotCore.Middlewares;

namespace FinBot.BotCore.Telegram.Commands {
    public class ParseCommandMiddleware : IMiddleware {
        private readonly ICommandParser _commandParser;

        public ParseCommandMiddleware(ICommandParser commandParser) {
            _commandParser = commandParser;
        }

        public async Task<MiddlewareData> InvokeAsync(MiddlewareData data, IMiddlewaresChain chain) {
            var commandInfo = await _commandParser.Parse(data);
            var newData = data.UpdateFeatures(f => f.Add<CommandFeature>(new CommandFeature(commandInfo)));
            return await chain.NextAsync(newData);
        }

    }
}