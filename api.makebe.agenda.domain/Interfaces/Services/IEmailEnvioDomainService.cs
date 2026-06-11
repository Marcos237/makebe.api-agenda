using api.makebe.agenda.domain.DTO;

namespace api.makebe.agenda.domain.Interfaces.Services
{
    public interface IEmailEnvioDomainService
    {
        Task GerarEmailsAgendamento(AgendamentoDTO agendamentoDTO);
    }
}
