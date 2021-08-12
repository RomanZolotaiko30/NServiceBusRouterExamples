using NServiceBus;
using NServiceBus.Router;
using NServiceBus.Transport;
using NServiceBusRouterExamples.Shared.Consts;
using Redactus.Communication.Commands;
using Redactus.Messaging.Commands;
using RouterWithCustomRules.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace RouterWithCustomRules.Handlers
{
    public sealed class HealthCheckHandler : BaseHandler
    {
        protected override PostroutingContext PostHandle(
            object message,
            TransportOperation commandResultReceivedTransportOperation,
            PostroutingContext context,
            TransportOperation sourceTransportOperation)
        {
            var postHandleContext = base.PostHandle(message, commandResultReceivedTransportOperation, context, sourceTransportOperation);

            var healthCheckResult = (HealthCheckResult)message;

            var updateWfaDeviceDataCommand = new UpdateWfaDeviceData
            {
                DeviceId = healthCheckResult.DeviceId,
                DeviceInfo = healthCheckResult.DeviceInfo,
                SystemStatuses = healthCheckResult.SystemStatuses,
                ReceivedOn = healthCheckResult.ReceivedOn
            };

            var updateProxyMonitoringDeviceDataCommand = new UpdateProxyMonitoringDeviceData
            {
                DeviceId = healthCheckResult.DeviceId,
                IpAddress = healthCheckResult.SystemStatuses?.InternetConnectionStatus?.IpAddress,
                ProxyIp = healthCheckResult.SystemStatuses?.ProxyServerStatus?.ProxyIp,
                ProxyPort = healthCheckResult.SystemStatuses?.ProxyServerStatus?.ProxyPort,
                SDKVersion = (healthCheckResult.DeviceInfo?.SDKVersion).GetValueOrDefault()
            };

            var commands = new List<NServiceBus.ICommand> { updateWfaDeviceDataCommand, updateProxyMonitoringDeviceDataCommand };
            var transportOperations = new List<TransportOperation>();

            foreach (var command in commands)
            {
                var queueName = command.GetType() == typeof(UpdateWfaDeviceData)
                    ? ApplicationContst.QueueNames.UpdateWfaDeviceDataQueueName
                    : ApplicationContst.QueueNames.UpdateProxyMonitoringDeviceData;

                var operation = command.ToTransportOperation(queueName, MessageIntentEnum.Send, sourceTransportOperation);

                transportOperations.Add(operation);
            }

            return new PostroutingContext(postHandleContext.DestinationEndpoint, postHandleContext.Messages.ToList().Concat(transportOperations).ToArray(), context);
        }
    }
}
