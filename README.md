# NServiceBusRouterExamples
AzureStorageQueueTransportEndpoint - an endpoind that is configured to fetch messages from the azure storage and send them to the router

AzureServiceBusTransportEndpoint - an endpoint that is configured to fetch messages from the azure service bus queue

NServiceBusRouter - contains a default NServiceBus router implementation that is configured to establish a communication between two endpoints with different types of transport.

RouterWithCustomRules - a router that uses a custom rule to intercept messages from the azure storage queue on the fly and transform them into other messages/events that will be sent/published to appropriate endpoints.
