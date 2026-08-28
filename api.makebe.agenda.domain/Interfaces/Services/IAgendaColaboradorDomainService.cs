using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.infra.crosscutting.Entidades;

namespace api.makebe.agenda.domain.Interfaces.Services
{
    public interface IAgendaColaboradorDomainService
    {
        Task<AgendaDTO> BuscarPorIdColaborador(int idColaborador);
    }
}
