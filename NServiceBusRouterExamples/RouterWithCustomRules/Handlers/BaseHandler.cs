using NServiceBus;
using NServiceBus.Router;
using NServiceBus.Transport;
using NServiceBusRouterExamples.Shared.Consts;
using Redactus.Common.Extensions;
using Redactus.Communication.Commands;
using Redactus.Messaging.Commands;
using Redactus.Wfa.Communication;
using RouterWithCustomRules.Extensions;

namespace RouterWithCustomRules.Handlers
{
    public class BaseHandler
    {
        public PostroutingContext Handle(object message, TransportOperation sourceTransportOperation, PostroutingContext context)
        {
            var commandCompletionResult = (CommandCompletionResult)message;

            var commandResultReceived = new ECommandResultReceived
            {
                CorrelationId = commandCompletionResult.DeviceId.ToGuid(),
                CommandId = commandCompletionResult.CommandId,
                DeviceId = commandCompletionResult.DeviceId,
                ReceivedOn = commandCompletionResult.ReceivedOn.GetValueOrDefault(),
                SerializedResult = commandCompletionResult.Serialize(),
                EncryptedFingerprint = commandCompletionResult.EncryptedFingerprint
            };

            var commandResultReceivedTransportOperation = commandResultReceived
                .ToTransportOperation(ApplicationContst.RouterEndpointNames.AzureServiceBusTopicName, MessageIntentEnum.Publish, sourceTransportOperation);

            return PostHandle(message, commandResultReceivedTransportOperation, context, sourceTransportOperation);
        }

        protected virtual PostroutingContext PostHandle(
            object message,
            TransportOperation commandResultReceivedTransportOperation,
            PostroutingContext context,
            TransportOperation sourceTransportOperation)
        {
            return new PostroutingContext(context.DestinationEndpoint, commandResultReceivedTransportOperation, context);
        }
    }
}
