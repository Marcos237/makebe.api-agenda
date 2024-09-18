using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;

namespace api.makebe.agenda.infra.data.Repositorys.Interfaces
{
    public interface ILojaRepository
    {
        Task<IEnumerable<LojaEnderecoDTO>> BuscarLojas(PaginacaoDTO<LojaEnderecoDTO> paginacao, string usuarioId);
        Task<Loja> BuscarLojaPorCodigo(int codigo);
        Task<int> Salvar(Loja loja);
        Task<Loja> Atualizar(Loja loja);
    }
}
