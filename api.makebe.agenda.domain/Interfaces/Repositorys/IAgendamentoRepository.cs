using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;

namespace api.makebe.agenda.domain.Interfaces.Repositorys
{
    public interface IAgendamentoRepository
    {
        Task<PaginacaoDTO<AgendamentoDTO>> BuscarPaginado(PaginacaoDTO<AgendamentoDTO> paginacao, string contaId);
        Task<AgendamentoDTO> BuscarPorId(int id);
        Task<int> Salvar(Agendamento agendamento);
        Task<bool> Atualizar(Agendamento agendamento);
        Task<bool> Desativar(int id);

    }
}
