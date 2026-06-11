using api.makebe.agenda.applications.Interfaces;
using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.infra.crosscutting.Notifications;
using AgendamentoPersistenciaEvent;
using MassTransit;

namespace api.makebe.agenda.applications.Consumers
{
    public class AgendamentoPersistidoConsumer : IConsumer<AgendamentoPersistidoEvent>
    {
        private readonly IAgendamentoApplicationService _agendamentoApplicationService;

        public AgendamentoPersistidoConsumer(IAgendamentoApplicationService agendamentoApplicationService)
        {
            _agendamentoApplicationService = agendamentoApplicationService;
        }

        public async Task Consume(ConsumeContext<AgendamentoPersistidoEvent> context)
        {
            var message = context.Message;
            if (message.Agendamento == null)
            {
                await context.RespondAsync(new AgendamentoPersistidoEvent
                {
                    Agendamento = null,
                    UsuarioIdEvent = message.UsuarioIdEvent,
                    Notifications = Enumerable.Empty<Notification>(),
                    DataEvento = DateTime.UtcNow
                });
                return;
            }

            var agendamento = new AgendamentoDTO
            {
                Id = message.Agendamento.Id,
                IdLoja = message.Agendamento.IdLoja,
                RazaoSocial = message.Agendamento.RazaoSocial,
                IdAgendaColaborador = message.Agendamento.IdAgendaColaborador,
                IdColaborador = message.Agendamento.IdColaborador,
                NomeColaborador = message.Agendamento.NomeColaborador,
                IdServico = message.Agendamento.IdServico,
                DescricaoServico = message.Agendamento.DescricaoServico,
                Valor = message.Agendamento.Valor,
                IdUsuario = message.Agendamento.IdUsuario,
                NomeUsuario = message.Agendamento.NomeUsuario,
                Data = message.Agendamento.DataInicioAgendamento.ToShortDateString(),
                DataInicioAgendamento = message.Agendamento.DataInicioAgendamento,
                DataTerminoAgendamento = message.Agendamento.DataTerminoAgendamento,
                DataInicioAgendamentoExtenso = message.Agendamento.DataInicioAgendamento.ToString(),
                PeriodoInativoInicio = message.Agendamento.PeriodoInativoInicio,
                PeriodoInativoFim = message.Agendamento.PeriodoInativoFim,
                Ativo = message.Agendamento.Ativo,
                Periodo = message.Agendamento.Periodo
            };

            var response = await _agendamentoApplicationService.Persistir(agendamento, message.UsuarioIdEvent ?? string.Empty);
            await context.RespondAsync(new AgendamentoPersistidoEvent
            {
                Agendamento = response.data == null ? null : new global::AgendamentoPersistenciaEvent.AgendamentoPersistenciaEvent
                {
                    Id = response.data.Id,
                    IdLoja = response.data.IdLoja,
                    RazaoSocial = response.data.RazaoSocial,
                    IdAgendaColaborador = response.data.IdAgendaColaborador,
                    IdColaborador = response.data.IdColaborador,
                    NomeColaborador = response.data.NomeColaborador,
                    IdServico = response.data.IdServico,
                    DescricaoServico = response.data.DescricaoServico,
                    Valor = response.data.Valor,
                    IdUsuario = response.data.IdUsuario,
                    NomeUsuario = response.data.NomeUsuario,
                    Data = response.data.Data,
                    DataInicioAgendamento = response.data.DataInicioAgendamento,
                    DataTerminoAgendamento = response.data.DataTerminoAgendamento,
                    DataInicioAgendamentoExtenso = response.data.DataInicioAgendamentoExtenso,
                    PeriodoInativoInicio = response.data.PeriodoInativoInicio,
                    PeriodoInativoFim = response.data.PeriodoInativoFim,
                    Ativo = response.data.Ativo,
                    Periodo = response.data.Periodo
                },
                UsuarioIdEvent = message.UsuarioIdEvent,
                Notifications = response.notifications ?? Enumerable.Empty<Notification>(),
                DataEvento = DateTime.UtcNow
            });
        }
    }
}
