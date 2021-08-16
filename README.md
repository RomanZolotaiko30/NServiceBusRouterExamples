# NServiceBusRouterExamples
AzureStorageQueueTransportEndpoint - an endpoint that is configured to fetch messages from the azure storage and send them to the router

AzureServiceBusTransportEndpoint - an endpoint that is configured to fetch messages from the azure service bus queue

NServiceBusRouter - contains a default NServiceBus router implementation that is configured to establish a communication between two endpoints with different types of transport.

RouterWithCustomRules - a router that uses a custom rule to intercept messages from the azure storage queue on the fly and transform them into other messages/events that will be sent/published to appropriate endpoints.

Our rule is a custom element that we inject into the NServiceBus.Router creation pipeline. We discovered this possibility during a detailed examination of the source code.
