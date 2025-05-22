using api.makebe.agenda.domain.Entidades;

namespace api.makebe.agenda.domain.Interfaces.Services
{
    public interface IAgendaDomainService
    {
        Task<int> Persitir(Agenda agenda);
        Task<bool> Desativar(int id);
        Task PreencherDiasSemana(Agenda agenda);
        Task BloquearAgendaHoje(Agenda agenda);
    }
}
