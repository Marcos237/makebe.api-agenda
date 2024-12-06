using api.makebe.agenda.domain.Entidades;

namespace api.makebe.agenda.domain.Interfaces.Repositorys
{
    public interface IServicosRepository
    {
        Task<IEnumerable<Servicos>> BuscarServicos();
    }
}
