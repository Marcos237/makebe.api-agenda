using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;

namespace api.makebe.agenda.domain.Interfaces.Repositorys
{
    public interface IServicosRepository
    {
        Task<IEnumerable<Servicos>> BuscarServicos(string contaId);
        Task<Servicos> BuscarPorId(int id);
        Task<PaginacaoDTO<ServicoDTO>> BuscarPaginado(PaginacaoDTO<ServicoDTO> paginacao, string contaId);
        Task<int> Salvar(Servicos servicos);
        Task<Servicos> Atualizar(Servicos servicos);
        Task<bool> Desativar(int id);   

    }
}
