using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;

namespace api.makebe.agenda.domain.Interfaces.Services
{
    public interface IColaboradorProfissionalDomainService
    {
        Task<PaginacaoDTO<ColaboradorProfissionalDTO>> BuscarPaginado(PaginacaoDTO<ColaboradorProfissionalDTO> paginacao, string contaId);
        Task<ColaboradorProfissionalDTO> BuscarPorId(int id);
        Task<int> Salvar(ColaboradorProfissional colaborador);
        Task<bool> Desativar(int id);
        Task<IEnumerable<ColaboradorProfissionalDTO>> BuscarPorConta(string contaId);
        Task<bool> BuscarAgendaVisible(int colaboradorId);
    }
}
