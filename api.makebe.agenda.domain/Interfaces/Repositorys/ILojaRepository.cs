using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;

namespace api.makebe.agenda.infra.data.Repositorys.Interfaces
{
    public interface ILojaRepository
    {
        Task<IEnumerable<LojaDTO>> BuscarTodos(string contaId);
        Task<PaginacaoDTO<LojaDTO>> BuscarLojas(PaginacaoDTO<LojaDTO> paginacao, string contaId);
        Task<LojaDTO> BuscarLojaPorCodigo(int codigo);
        Task<int> Salvar(Loja loja);
        Task<Loja> Atualizar(Loja loja);
        Task<bool> Desativar(int id);
    }
}
