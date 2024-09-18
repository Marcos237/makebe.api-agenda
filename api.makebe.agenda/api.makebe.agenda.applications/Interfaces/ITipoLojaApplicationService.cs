using api.makebe.agenda.applications.Models.Responses;
using api.makebe.agenda.domain.Entidades;

namespace api.makebe.agenda.applications.Interfaces
{
    public interface ITipoLojaApplicationService
    {
        Task<ResponseModel<TipoLoja>> BuscarTodos();
    }
}
