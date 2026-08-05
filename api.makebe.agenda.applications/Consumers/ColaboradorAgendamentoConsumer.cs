using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Interfaces.Repositorys;
using ColaboradorAgendamentoEvent;
using MassTransit;

namespace api.makebe.agenda.applications.Consumers
{
    public class ColaboradorAgendamentoConsumer : IConsumer<ColaboradorAgendamentoPublicadoEvent>
    {
        private readonly IAgendaColaboradorRepository _agendaColaboradorRepository;
        private readonly IAgendamentoColaboradorRepository _agendamentoColaboradorRepository;
        private readonly IServicosRepository _servicosRepository;

        public ColaboradorAgendamentoConsumer(
            IAgendaColaboradorRepository agendaColaboradorRepository,
            IAgendamentoColaboradorRepository agendamentoColaboradorRepository,
            IServicosRepository servicosRepository)
        {
            _agendaColaboradorRepository = agendaColaboradorRepository;
            _agendamentoColaboradorRepository = agendamentoColaboradorRepository;
            _servicosRepository = servicosRepository;
        }

        public async Task Consume(ConsumeContext<ColaboradorAgendamentoPublicadoEvent> context)
        {
            var message = context.Message;
            var agendas = await _agendaColaboradorRepository.BuscarAgendamentosPorColaboradorId(message.ColaboradorId)
                ?? Enumerable.Empty<AgendaDTO>();
            var agendamentos = await _agendamentoColaboradorRepository.BuscarAgendamentosPorColaboradorId(message.ColaboradorId)
                ?? Enumerable.Empty<AgendamentoDTO>();

            var servicos = await _servicosRepository.BuscarServicosPorColaboradorId(message.ColaboradorId);
            await context.RespondAsync(new ColaboradorAgendamentoPublicadoEvent
            {
                ColaboradorId = message.ColaboradorId,
                DataEvento = DateTime.UtcNow,
                Agendamentos = await Task.WhenAll(agendas!.Select(async x =>
                {
                    return new global::ColaboradorAgendamentoEvent.ColaboradorAgendamentoEvent
                    {
                        Id = x.Id,
                        IdAgendaColaborador = x.IdAgendaColaborador,
                        IsBloqueadoHoje = x.IsBloqueadoHoje,
                        AgendaAbertaInicio = x.AgendaAbertaInicio,
                        AgendaAbertaFim = x.AgendaAbertaFim,
                        AgendaBloqueadaInicio = x.AgendaBloqueadaInicio,
                        AgendaBloqueadaFim = x.AgendaBloqueadaFim,
                        UsuarioId = x.UsuarioId,
                        DataInicioAgendamento = x.DataInicioAgendamento,
                        IdAgendaSemanaInicio = x.IdAgendaSemanaInicio,
                        IdAgendaSemanaFim = x.IdAgendaSemanaFim,
                        DataTerminoAgendamento = x.DataTerminoAgendamento,
                        Servicos = servicos.Select(servico => new ServicoEvent
                        {
                            Id = servico.Id,
                            Descricao = servico.Descricao,
                            Valor = servico.Valor,
                            Periodo = servico.Periodo
                        })
                        .ToList(),
                        Agendamentos = agendamentos
                            .Where(agendamento => agendamento.IdAgendaColaborador == x.IdAgendaColaborador)
                            .Select(agendamento => new global::ColaboradorAgendamentoEvent.AgendamentoEvent
                            {
                                Id = agendamento.Id,
                                IdAgendaColaborador = agendamento.IdAgendaColaborador,
                                IdServico = agendamento.IdServico,
                                IdUsuario = agendamento.IdUsuario,
                                IdColaborador = int.TryParse(agendamento.IdColaborador, out var idColaborador) ? idColaborador : 0,
                                DescricaoServico = agendamento.DescricaoServico,
                                Valor = agendamento.Valor,
                                DataInicioAgendamento = agendamento.DataInicioAgendamento,
                                DataTerminoAgendamento = agendamento.DataTerminoAgendamento,
                                Periodo = agendamento.Periodo,
                            })
                            .ToList()
                    };
                }))
            });
        }
    }
}
