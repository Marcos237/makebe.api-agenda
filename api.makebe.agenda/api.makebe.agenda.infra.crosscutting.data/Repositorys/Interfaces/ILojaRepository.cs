using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;

namespace api.makebe.agenda.infra.data.Repositorys.Interfaces
{
    public interface ILojaRepository
    {
        Task<PaginacaoDTO<Loja>> BuscarLojas(PaginacaoDTO<Loja> paginacao);
        Task<Loja> BuscarLojaPorCodigo(int codigo);
        Task<Loja> BuscarLojaPorCNPJ(string cnpj);
        Task<int> Salvar(Loja loja);
        Task<Loja> Atualizar(Loja loja);
    }
}
