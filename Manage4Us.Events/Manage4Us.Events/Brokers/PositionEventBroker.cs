using Azure.Messaging.ServiceBus;
using Manage4Us.Events.Domain;
using Manage4Us.Events.Interfaces;
using System.Text.Json;

namespace Manage4Us.Events.Brokers
{
    public class PositionEventBroker : IPositionEventBroker
    {
        private const string PositionTopicName = "m4us-position";
        private readonly ServiceBusSender _positionSender;

        public PositionEventBroker(EventBrokerConfigs configs)
        {
            if (string.IsNullOrEmpty(configs.ConString))
                throw new ArgumentException(nameof(configs.ConString));

            var clientOptions = new ServiceBusClientOptions
            {
                TransportType = ServiceBusTransportType.AmqpWebSockets,
                RetryOptions = new ServiceBusRetryOptions
                {
                    MaxRetries = 10,
                    Delay = TimeSpan.FromSeconds(10),
                    Mode = ServiceBusRetryMode.Exponential
                }
            };

            var client = new ServiceBusClient(configs.ConString, clientOptions);
            _positionSender = client.CreateSender(PositionTopicName);
        }

        public async Task PushPositionAsync(double latitude, double longitude, int employeeId, int loginId)
        {
            var json = JsonSerializer.Serialize(new PositionEvent
            {
                Latitude = latitude,
                Longitude = longitude,
                EmployeeId = employeeId,
                LoginId = loginId
            });
            await _positionSender.SendMessageAsync(new ServiceBusMessage(json));
        }
    }
}
