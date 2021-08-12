using NServiceBus;
using NServiceBus.Routing;
using NServiceBus.Transport;
using NServiceBusRouterExamples.Shared.Consts;
using RouterWithCustomRules.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace RouterWithCustomRules.Extensions
{
    public static class TransportOperationExtensions
    {
        public static TransportOperation ToTransportOperation<T>(
            this T message,
            string destinationEndpoint,
            MessageIntentEnum messageIntent,
            TransportOperation sourceTransportOperation)
            where T : class
        {
            var headers = new Dictionary<string, string>
            {
                { ApplicationContst.NServiceBusHeaderNames.DestinationEndpoint, destinationEndpoint },
                { Headers.MessageIntent, messageIntent.ToString() },
            };

            var serializedEvent = CommandHelper.Serialize(message);
            var encodedBody = Encoding.UTF8.GetBytes(serializedEvent);

            var outgoingMessage = new OutgoingMessage(Guid.NewGuid().ToString(), headers, encodedBody);

            return new TransportOperation(
              outgoingMessage,
              new UnicastAddressTag(destinationEndpoint),
              sourceTransportOperation.RequiredDispatchConsistency,
              sourceTransportOperation.DeliveryConstraints);
        }
    }
}
