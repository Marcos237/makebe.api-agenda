using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;

namespace api.makebe.agenda.infra.data.Repositorys.Interfaces
{
    public interface ILojaRepository
    {
        Task<PaginacaoDTO<LojaEnderecoDTO>> BuscarLojas(PaginacaoDTO<LojaEnderecoDTO> paginacao, string usuarioId);
        Task<LojaEnderecoDTO> BuscarLojaPorCodigo(int codigo);
        Task<int> Salvar(Loja loja);
        Task<Loja> Atualizar(Loja loja);
        Task<bool> Desativar(int id);
    }
}
