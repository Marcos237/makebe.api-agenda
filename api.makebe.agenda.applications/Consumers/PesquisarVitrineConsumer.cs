using api.makebe.agenda.applications.Interfaces;
using MassTransit;
using PesquisarVitrineEvent;

namespace api.makebe.agenda.applications.Consumers
{
    public class PesquisarVitrineConsumer : IConsumer<PesquisarVitrineMessage>
    {
        private readonly IVitrineService _service;

        public PesquisarVitrineConsumer(IVitrineService service)
        {
            _service = service;
        }

        public async Task Consume(ConsumeContext<PesquisarVitrineMessage> context)
        {
            var resultado = await _service.PesquisarAsync(
                context.Message.Termo,
                context.CancellationToken);

            await context.RespondAsync<IPesquisarVitrineResponse>(new
            {
                Itens = resultado
            });
        }
    }
}
