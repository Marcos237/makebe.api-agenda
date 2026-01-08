using api.makebe.agenda.domain.DTO;

namespace api.makebe.agenda.domain.Interfaces.Repositorys
{
    public  interface IAgendaLojaRepository
    {
        Task<IEnumerable<AgendaDTO>> BuscarAgendaLojaDentroDoBloqueio(DateTime dataInicio, DateTime DataFim, int id);
    }
}
