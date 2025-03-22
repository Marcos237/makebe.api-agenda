using api.makebe.agenda.domain.Entidades;

namespace api.makebe.agenda.domain.Interfaces.Services
{
    public interface IContaServicoDomainService
    {
        Task<int> Salvar(ContaServico contaServico, int id);
        Task<IEnumerable<ContaServico>> BuscarServicoPorConta(string contaId);
    }
}
