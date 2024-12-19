using api.makebe.agenda.infra.crosscutting.Entidades.Constants;
using api.makebe.agenda.infra.crosscutting.Events.Interfaces;
using api.makebe.agenda.infra.crosscutting.Notifications.Interfaces;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace api.makebe.agenda.infra.crosscutting.Events
{
    public sealed class BusEvent : IBusEvent
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IServiceProvider _serviceProvider;
        private readonly INotificationContext _notificationContext;

        public BusEvent(IPublishEndpoint publishEndpoint, IServiceProvider serviceProvider, INotificationContext notificationContext)
        {
            _publishEndpoint = publishEndpoint;
            _serviceProvider = serviceProvider;
            _notificationContext = notificationContext;
        }

        public Task PublishAsync<T>(T item, CancellationToken cancellationToken = default) where T : class
        {
            var retorno = _publishEndpoint.Publish(item, cancellationToken);
            return Task.FromResult(retorno);
        }

        public async Task<TResult> RequestAsync<TRequest, TResult>(TRequest request, TimeSpan timeout)
            where TRequest : class
            where TResult : class
        {
            var requestClient = _serviceProvider.GetRequiredService<IRequestClient<TRequest>>();

            try
            {
                var response = await requestClient.GetResponse<TResult>(request ?? null!, timeout: timeout);
                return response.Message;
            }
            catch (TimeoutException ex)
            {
                _notificationContext.AddNotification(nameof(ex), ex.Message.ToString(), isValidate: true);
                throw;
            }
        }

    }
}
