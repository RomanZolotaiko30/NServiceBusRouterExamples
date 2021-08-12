using NServiceBus;
using NServiceBus.Azure.Transports.WindowsAzureStorageQueues;
using NServiceBus.Features;
using NServiceBusRouterExamples.Shared.Consts;
using Redactus.Communication.Commands;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AzureStorageQueueTransportEndpoint
{
    public static class Program
    {
        static async Task Main(string[] args)
        {
            var endpointConfiguration = new EndpointConfiguration(ApplicationContst.QueueNames.AzureStorageQueueName);

            endpointConfiguration.DisableFeature<TimeoutManager>();
            endpointConfiguration.UseSerialization<NewtonsoftSerializer>();
            endpointConfiguration.EnableInstallers();

            var transport = endpointConfiguration.UseTransport<AzureStorageQueueTransport>();

            transport.ConnectionString(ApplicationContst.ConnectionStrings.AzureStorageConnectionString);
            transport.BatchSize(1);
            transport.DegreeOfReceiveParallelism(1);
            transport.DisablePublishing();

            transport.UnwrapMessagesWith(queueMessage =>
            {
                return new MessageWrapper
                {
                    Id = queueMessage.MessageId,
                    Headers = new Dictionary<string, string>(),
                    Body = Convert.FromBase64String(queueMessage.MessageText)
                };
            });

            var bridge = transport.Routing().ConnectToBridge(ApplicationContst.RouterEndpointNames.RouterName);

            bridge.RouteToEndpoint(typeof(HealthCheckResult), ApplicationContst.RouterEndpointNames.AzureServiceBusEndpointName);

            var endpointInstance = await Endpoint.Start(endpointConfiguration).ConfigureAwait(false);

            Console.WriteLine("Listeting to messages from the azure storage queue");
            Console.ReadLine();

            await endpointInstance.Stop().ConfigureAwait(false);
        }
    }
}
