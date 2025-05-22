using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;

namespace api.makebe.agenda.domain.Interfaces.Services
{
    public interface IAgendaContextDomainService<T>where T : class
    {
        Task<PaginacaoDTO<AgendaDTO>> BuscarPaginado(PaginacaoDTO<AgendaDTO> paginacao, string contaId);
        Task<AgendaDTO> BuscarPorId(int id);
        Task<int> Persistir(T item);
    }
}
