using api.makebe.agenda.applications.Models.Payloads;
using api.makebe.agenda.applications.Models.Responses;
using api.makebe.agenda.domain.DTO;
using api.makebesession.infra.crosscutting.Events.Usuarios;

namespace api.makebe.agenda.applications.Interfaces
{
    public interface IColaboradorApplicationService
    {
        Task<ResponseModel<ColaboradorDTO>> BuscarUsuario(ColaboradorPayload usuarioPayload, string usuario);
        Task<ResponseModel<ColaboradorDTO>> SalvarUsuario(ColaboradorPayload usuarioPayload, string usuario);
    }
}
