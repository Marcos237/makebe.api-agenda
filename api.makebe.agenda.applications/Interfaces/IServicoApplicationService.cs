using api.makebe.agenda.applications.Models.Responses;
using api.makebe.agenda.domain.Entidades;

namespace api.makebe.agenda.applications.Interfaces
{
    public interface IServicoApplicationService
    {
        Task<ResponseModel<Servicos>> BuscarServicos(string usuarioId);
    }
}
