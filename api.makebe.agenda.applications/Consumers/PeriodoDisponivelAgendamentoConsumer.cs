using api.makebe.agenda.applications.Interfaces;
using api.makebe.agenda.domain.DTO;
using MassTransit;
using PeriodoDisponivelAgendamentoEvent;

namespace api.makebe.agenda.applications.Consumers
{
    public class PeriodoDisponivelAgendamentoConsumer : IConsumer<PeriodoDisponivelAgendamentoPublicadoEvent>
    {
        private readonly IPeriodoDisponivelAgendamentoApplicationService _periodoDisponivelAgendamentoApplicationService;

        public PeriodoDisponivelAgendamentoConsumer(IPeriodoDisponivelAgendamentoApplicationService periodoDisponivelAgendamentoApplicationService)
        {
            _periodoDisponivelAgendamentoApplicationService = periodoDisponivelAgendamentoApplicationService;
        }

        public async Task Consume(ConsumeContext<PeriodoDisponivelAgendamentoPublicadoEvent> context)
        {
            var message = context.Message;
            var request = new PeriodoDisponivelRequestDTO
            {
                IdServico = message.IdServico,
                IdColaborador = message.IdColaborador,
                Data = message.Data
            };

            var periodos = await _periodoDisponivelAgendamentoApplicationService.BuscarPeriodosDisponiveis(request);

            await context.RespondAsync(new PeriodoDisponivelAgendamentoPublicadoEvent
            {
                IdServico = message.IdServico,
                IdColaborador = message.IdColaborador,
                Data = message.Data,
                DataEvento = DateTime.UtcNow,
                Periodos = periodos.Select(periodo => new PeriodoEvent
                {
                    Inicio = periodo.Inicio,
                    Fim = periodo.Fim,
                    IsAgendado = periodo.IsAgendado
                })
            });
        }
    }
}
