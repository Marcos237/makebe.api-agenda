using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;

namespace api.makebe.agenda.domain.Interfaces.Repositorys
{
    public interface IAgendaContextRepository<T> where T : class
    {
        Task<PaginacaoDTO<AgendaDTO>> BuscarPaginado(PaginacaoDTO<AgendaDTO> paginacao, string contaId);
        Task<AgendaDTO> BuscarPorId(int id);
        Task<IEnumerable<AgendaDTO>> BuscarAgendaLojaDentroDoBloqueio(DateTime dataInicio, DateTime DataFim, int id);
        Task<int> Salvar(T item);
        Task<bool> Atualizar(T item);
    }
}
