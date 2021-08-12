using NServiceBus;
using Redactus.Communication.Commands;
using System;
using System.Threading.Tasks;

namespace AzureServiceBusTransportEndpoint
{
    public sealed class HealthCheckHandler : IHandleMessages<HealthCheckResult>
    {
        public Task Handle(HealthCheckResult message, IMessageHandlerContext context)
        {
            Console.WriteLine("Received a HealthCheck from the azure service bus queue.");

            return Task.CompletedTask;
        }
    }
}
