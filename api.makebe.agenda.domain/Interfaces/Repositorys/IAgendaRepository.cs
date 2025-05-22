using api.makebe.agenda.domain.Entidades;

namespace api.makebe.agenda.domain.Interfaces.Repositorys
{
    public interface IAgendaRepository
    {
        Task<Agenda> BuscarPoId(int id);
        Task<int> Salvar(Agenda agenda);
        Task<bool> Atualizar(Agenda agenda);
        Task<bool> Desativar(int id);
    }
}
