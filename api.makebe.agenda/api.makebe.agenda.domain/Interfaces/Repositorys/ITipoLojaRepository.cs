using api.makebe.agenda.domain.Entidades;

namespace api.makebe.agenda.domain.Interfaces.Repositorys
{
    public interface ITipoLojaRepository
    {
        Task<IEnumerable<TipoLoja>> BuscarTodos();
    }
}
