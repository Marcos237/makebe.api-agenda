using api.makebe.agenda.domain.Entidades;

namespace api.makebe.agenda.applications.Interfaces
{
    public interface IServicoApplicationService
    {
        Task<IEnumerable<Servicos>> BuscarServicos();
    }
}
