using api.makebe.agenda.domain.Entidades;

namespace api.makebe.agenda.domain.Interfaces.Repositorys
{
    public interface IContaServicoRepository
    {
        Task<int> Salvar(ContaServico contaServico);
        Task<IEnumerable<ContaServico>> BuscarServicoPorConta(string contaId);
        Task<bool> Atualizar(ContaServico contaServico);
    }
}
