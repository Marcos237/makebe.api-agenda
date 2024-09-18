using api.makebe.agenda.domain.Entidades;

namespace api.makebe.agenda.domain.Interfaces.Services
{
    public interface ITipoLojaDomainService
    {
        Task<IEnumerable<TipoLoja>> BuscarTodos();
    }
}
