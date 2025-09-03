using api.makebe.agenda.domain.DTO;

namespace api.makebe.agenda.domain.Interfaces
{
    public interface IFiltrosAgendamentoDomainService
    {
        IEnumerable<AgendamentoDTO> FiltrarPorNomes(PaginacaoDTO<AgendamentoDTO> paginacao);
        IEnumerable<AgendamentoDTO> FiltrarPorDatas(PaginacaoDTO<AgendamentoDTO> paginacao);
    }
}
