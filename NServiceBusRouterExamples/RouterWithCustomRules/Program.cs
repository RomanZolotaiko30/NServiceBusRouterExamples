using NServiceBus;
using NServiceBus.Azure.Transports.WindowsAzureStorageQueues;
using NServiceBus.Router;
using NServiceBusRouterExamples.Shared.Consts;
using RouterWithCustomRules.Rules;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RouterWithCustomRules
{
    public static class Program
    {
        static async Task Main(string[] args)
        {
            var routerConfiguration = new RouterConfiguration(ApplicationContst.QueueNames.AzureStorageQueueName);

            var azureStorageQueue = routerConfiguration
                .AddInterface<AzureStorageQueueTransport>(ApplicationContst.NServiceBusTransportInterfaceNames.AzureStorageQueueTransportInterface, transport =>
                {
                    transport.ConnectionString(ApplicationContst.ConnectionStrings.AzureStorageConnectionString);
                    transport.BatchSize(4);
                    transport.DegreeOfReceiveParallelism(2);
                    transport.DisablePublishing();

                    transport.UnwrapMessagesWith(queueMessage =>
                    {
                        return new MessageWrapper
                        {
                            IdForCorrelation = null,
                            Id = queueMessage.MessageId,
                            Headers = new Dictionary<string, string>
                            {
                                { Headers.MessageId, queueMessage.MessageId },
                                { ApplicationContst.NServiceBusHeaderNames.DestinationEndpoint, ApplicationContst.RouterEndpointNames.AzureServiceBusEndpointName},
                                { Headers.MessageIntent, "Send" },
                            },
                            Body = Convert.FromBase64String(queueMessage.MessageText),
                            Recoverable = true
                        };
                    });
                });

            var azureServiceBusTransportInterface = routerConfiguration
                 .AddInterface<AzureServiceBusTransport>(ApplicationContst.NServiceBusTransportInterfaceNames.AzureServiceBusTransportInterface, transport =>
                 {
                     transport.ConnectionString(ApplicationContst.ConnectionStrings.AzureServiceBusConnectionString);
                 });

            routerConfiguration.AddRule(_ => new PostroutingSendingRule());

            var staticRouting = routerConfiguration.UseStaticRoutingProtocol();

            staticRouting
                .AddForwardRoute(ApplicationContst.NServiceBusTransportInterfaceNames.AzureStorageQueueTransportInterface, ApplicationContst.NServiceBusTransportInterfaceNames.AzureServiceBusTransportInterface);

            var router = Router.Create(routerConfiguration);

            await router.Start().ConfigureAwait(false);

            Console.WriteLine("Custom router with rules started.");
            Console.ReadKey();

            await router.Stop().ConfigureAwait(false);
        }
    }
}
