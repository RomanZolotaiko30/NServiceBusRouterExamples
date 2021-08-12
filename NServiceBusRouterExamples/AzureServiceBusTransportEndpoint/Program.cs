using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NServiceBus;
using NServiceBus.Features;
using NServiceBusRouterExamples.Shared.Consts;
using System;
using System.Threading.Tasks;

namespace AzureServiceBusTransportEndpoint
{
   public static class Program
    {
        static async Task Main(string[] args)
        {
            var endpointConfiguration = new EndpointConfiguration(ApplicationContst.RouterEndpointNames.AzureServiceBusEndpointName);

            endpointConfiguration.EnableInstallers();

            endpointConfiguration.UseSerialization<NewtonsoftSerializer>()
                .Settings(new JsonSerializerSettings
                {
                    Formatting = Formatting.None,
                    TypeNameHandling = TypeNameHandling.All,
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });

            endpointConfiguration.DisableFeature<TimeoutManager>();

            var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
            transport.ConnectionString(ApplicationContst.ConnectionStrings.AzureServiceBusConnectionString);

            var endpointInstance = await Endpoint.Start(endpointConfiguration).ConfigureAwait(true);

            Console.WriteLine("Listeting to messages from the azure service bus queue.");
            Console.ReadKey();

            await endpointInstance.Stop().ConfigureAwait(false);
        }
    }
}
