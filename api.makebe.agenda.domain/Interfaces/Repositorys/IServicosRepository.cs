using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;

namespace api.makebe.agenda.domain.Interfaces.Repositorys
{
    public interface IServicosRepository
    {
        Task<IEnumerable<Servico>> BuscarServicos(string contaId);
        Task<Servico> BuscarPorId(int id);
        Task<IEnumerable<Servico>> BuscarServicosPorColaboradorId(int id);
        Task<PaginacaoDTO<ServicoDTO>> BuscarPaginado(PaginacaoDTO<ServicoDTO> paginacao, string contaId);
        Task<int> Salvar(Servico servicos);
        Task<Servico> Atualizar(Servico servicos);
        Task<bool> Desativar(int id);   

    }
}
