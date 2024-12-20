using api.makebe.agenda.domain.Entidades;

namespace api.makebe.agenda.domain.Interfaces.Services
{
    public interface IServicosDomainService
    {
        Task<IEnumerable<Servicos>> BuscarServicos();
    }
}
