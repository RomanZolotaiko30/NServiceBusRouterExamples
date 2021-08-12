using NServiceBus;
using NServiceBus.Router;
using NServiceBusRouterExamples.Shared.Consts;
using System;
using System.Threading.Tasks;

namespace NServiceBusRouter
{
    public static class Program
    {
        static async Task Main(string[] args)
        {
            var routerConfiguration = new RouterConfiguration(ApplicationContst.RouterEndpointNames.RouterName);

            var azureStorageQueueTransportInterface = routerConfiguration
                .AddInterface<AzureStorageQueueTransport>(ApplicationContst.NServiceBusTransportInterfaceNames.AzureStorageQueueTransportInterface, transport =>
            {
                transport.ConnectionString(ApplicationContst.ConnectionStrings.AzureStorageConnectionString);
            });

            var azureServiceBusTransportInterface = routerConfiguration
                .AddInterface<AzureServiceBusTransport>(ApplicationContst.NServiceBusTransportInterfaceNames.AzureServiceBusTransportInterface, transport =>
            {
                transport.ConnectionString(ApplicationContst.ConnectionStrings.AzureServiceBusConnectionString);
            });

            routerConfiguration.AutoCreateQueues();

            var staticRouting = routerConfiguration.UseStaticRoutingProtocol();

            staticRouting
                .AddForwardRoute(ApplicationContst.NServiceBusTransportInterfaceNames.AzureStorageQueueTransportInterface, ApplicationContst.NServiceBusTransportInterfaceNames.AzureServiceBusTransportInterface);

            var router = Router.Create(routerConfiguration);

            await router.Start().ConfigureAwait(false);

            Console.WriteLine("Router started.");
            Console.ReadKey();

            await router.Stop().ConfigureAwait(false);
        }
    }
}
