namespace api.makebe.agenda.infra.crosscutting.Events.Interfaces
{
    public interface IBusEvent
    {
        Task PublishAsync<T>(T item, CancellationToken cancellationToken = default) where T : class;
        Task<TResult> RequestAsync<TRequest, TResult>(TRequest request,TimeSpan timeout)
            where TRequest : class
            where TResult : class;
    }

}
