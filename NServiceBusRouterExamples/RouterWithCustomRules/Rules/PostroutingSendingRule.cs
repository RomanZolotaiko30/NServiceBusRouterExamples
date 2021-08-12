using NServiceBus.Router;
using Redactus.Communication.Commands;
using RouterWithCustomRules.Handlers;
using RouterWithCustomRules.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RouterWithCustomRules.Rules
{
    public sealed class PostroutingSendingRule : IRule<PostroutingContext, PostroutingContext>
    {
        private readonly Dictionary<Type, BaseHandler> _handlers;

        public PostroutingSendingRule()
        {
            _handlers = new Dictionary<Type, BaseHandler>
            {
                { typeof(HealthCheckResult), new HealthCheckHandler() }
            };
        }

        public async Task Invoke(PostroutingContext context, Func<PostroutingContext, Task> next)
        {
            foreach (var message in context.Messages)
            {
                var deserializedMessage = CommandHelper.DeserializeMessageToObject(message.Message.Body);

                var updatedContext = _handlers[deserializedMessage.GetType()].Handle(deserializedMessage, message, context);

                await next(updatedContext).ConfigureAwait(false);
            }
        }
    }
}
