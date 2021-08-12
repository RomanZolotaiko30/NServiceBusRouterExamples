namespace NServiceBusRouterExamples.Shared.Consts
{
    public static class ApplicationContst
    {
        public static class ConnectionStrings
        {
            //Don't forget to start azure storage emulator in case if debugging on local env
            public static string AzureStorageConnectionString => "UseDevelopmentStorage=true";

            public static string AzureServiceBusConnectionString => "Insert sb connection string here";
        }

        public static class QueueNames
        {
            //Be careful with queue name on real evironments, usually this name is reserved by real result queue
            public static string AzureStorageQueueName => "results";

            public static string UpdateWfaDeviceDataQueueName => "UpdateWfaDeviceDataQueue";

            public static string UpdateProxyMonitoringDeviceData => "UpdateProxyMonitoringDeviceDataQueue";
        }

        public static class RouterEndpointNames
        {
            public static string RouterName => "NServiceBusRouter";

            public static string AzureServiceBusEndpointName => "AzureServiceBusTransportEndpoint";

            public static string AzureServiceBusTopicName => "NServiceBusExamplesTopic";
        }

        public static class NServiceBusTransportInterfaceNames
        {
            public static string AzureStorageQueueTransportInterface => "AzureStorageQueueTransportInterface=true";

            public static string AzureServiceBusTransportInterface => "AzureServiceBusTransportInterface=true";
        }

        public static class NServiceBusHeaderNames
        {
            public static string DestinationEndpoint => "NServiceBus.Bridge.DestinationEndpoint";
        }
    }
}
