using api.makebe.agenda.domain.Interfaces.Repositorys;
using DesativarAgendamentoEvent;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace api.makebe.agenda.applications.Consumers
{
    public class DesativarAgendamentoConsumer : IConsumer<DesativarAgendamentoMessage>
    {
        private readonly IAgendamentoRepository _agendamentoRepository;
        private readonly ILogger<DesativarAgendamentoConsumer> _logger;

        public DesativarAgendamentoConsumer(
            IAgendamentoRepository agendamentoRepository,
            ILogger<DesativarAgendamentoConsumer> logger)
        {
            _agendamentoRepository = agendamentoRepository;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<DesativarAgendamentoMessage> context)
        {
            var id = context.Message.Id;

            if (id <= 0)
            {
                _logger.LogWarning("Id de agendamento inválido para desativação: {Id}", id);
                await context.RespondAsync(new DesativarAgendamentoResponse
                {
                    Sucesso = false
                });
                return;
            }

            try
            {
                var desativado = await _agendamentoRepository.Desativar(id);

                if (desativado)
                {
                    _logger.LogInformation("Agendamento {Id} desativado com sucesso.", id);
                }
                else
                {
                    _logger.LogWarning("Nenhum agendamento encontrado para desativar. Id: {Id}", id);
                }

                await context.RespondAsync(new DesativarAgendamentoResponse
                {
                    Sucesso = desativado
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao desativar o agendamento {Id}.", id);
                throw;
            }
        }
    }
}
