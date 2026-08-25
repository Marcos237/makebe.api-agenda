using api.makebe.agenda.applications.Interfaces;
using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Extensions;
using api.makebe.agenda.domain.Helpers;
using api.makebe.agenda.infra.crosscutting.Entidades;
using api.makebe.agenda.infra.crosscutting.Services.Interfaces;
using MassTransit;
using MeusAgendamentosEvent;
using UsuariosEvent;

namespace api.makebe.agenda.applications.Consumers
{
    public class MeusAgendamentosConsumer : IConsumer<MeusAgendamentosPublicadoEvent>
    {
        private readonly IAgendamentoApplicationService _agendamentoApplicationService;
        private readonly IUsuarioEventCrossCuttingService _usuarioEventCrossCuttingService;

        public MeusAgendamentosConsumer(
            IAgendamentoApplicationService agendamentoApplicationService,
            IUsuarioEventCrossCuttingService usuarioEventCrossCuttingService)
        {
            _agendamentoApplicationService = agendamentoApplicationService;
            _usuarioEventCrossCuttingService = usuarioEventCrossCuttingService;
        }

        public async Task Consume(ConsumeContext<MeusAgendamentosPublicadoEvent> context)
        {
            var message = context.Message;
            var paginacao = new PaginacaoDTO<AgendamentoConsultaDTO>
            {
                quantidadePagina = message.Paginacao.quantidadePagina,
                totalPaginas = message.Paginacao.totalPaginas,
                total = message.Paginacao.total,
                paginaAtual = message.Paginacao.paginaAtual,
                registroInicial = message.Paginacao.registroInicial
            };
            var agendamentosPaginados = await _agendamentoApplicationService.BuscarMeusAgendamentos(paginacao, message.UsuarioIdEvent ?? string.Empty);
            var agendamentos = agendamentosPaginados.objetos?.ToList() ?? new List<AgendamentoConsultaDTO>();
            var colaboradoresPorUsuarioId = new Dictionary<string, string?>();

            foreach (var agendamento in agendamentos)
            {
                agendamento.EhDesativado = agendamento.CalcularEhDesativado();

                if (string.IsNullOrWhiteSpace(agendamento.IdColaboradorUsuario))
                    continue;

                if (!colaboradoresPorUsuarioId.TryGetValue(agendamento.IdColaboradorUsuario, out var nomeColaborador))
                {
                    var usuarioEvent = new UsuarioConsultadoPorIdEvent
                    {
                        Id = PropiedadesHelper.ParseGuidOrDefault(agendamento.IdColaboradorUsuario)
                    };

                    var usuario = await _usuarioEventCrossCuttingService.BuscarUsuarioPorId(usuarioEvent);
                    nomeColaborador = usuario.UsuarioConsultadoRetorno?.Nome;
                    colaboradoresPorUsuarioId[agendamento.IdColaboradorUsuario] = nomeColaborador;
                }

                agendamento.NomeColaborador = nomeColaborador;
            }

            await context.RespondAsync(new MeusAgendamentosPublicadoEvent
            {
                UsuarioIdEvent = message.UsuarioIdEvent,
                DataEvento = DateTime.Now,
                Paginacao = new PaginacaoEvent<MeuAgendamentoEvent>
                {
                    quantidadePagina = agendamentosPaginados.quantidadePagina,
                    totalPaginas = agendamentosPaginados.totalPaginas,
                    total = agendamentosPaginados.total,
                    paginaAtual = agendamentosPaginados.paginaAtual,
                    registroInicial = agendamentosPaginados.registroInicial,
                    objetos = agendamentos.Select(agendamento => new MeuAgendamentoEvent
                    {
                        Id = agendamento.Id,
                        IdUsuario = agendamento.IdUsuario,
                        DataInicioAgendamento = agendamento.DataInicioAgendamento,
                        DataTerminoAgendamento = agendamento.DataTerminoAgendamento,
                        DescricaoServico = agendamento.DescricaoServico,
                        IdColaborador = agendamento.IdColaborador,
                        IdColaboradorUsuario = agendamento.IdColaboradorUsuario,
                        NomeColaborador = agendamento.NomeColaborador,
                        EhDesativado = agendamento.EhDesativado
                    })
                }
            });
        }
    }
}
