using NServiceBus;
using Redactus.Communication.Commands;
using System;
using System.Threading.Tasks;

namespace AzureStorageQueueTransportEndpoint
{
    public sealed class HealthCheckHandler : IHandleMessages<HealthCheckResult>
    {
        public async Task Handle(HealthCheckResult message, IMessageHandlerContext context)
        {
            Console.WriteLine("Received a HealthCheck from the azure storage queue");

            await context.Send(message).ConfigureAwait(false);
        }
    }
}
