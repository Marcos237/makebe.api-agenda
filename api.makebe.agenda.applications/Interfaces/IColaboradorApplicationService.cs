using api.makebe.agenda.applications.Models.Payloads;
using api.makebe.agenda.applications.Models.Responses;
using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.infra.crosscutting.Entidades;
using ContasEvent;
using UsuariosEvent;

namespace api.makebe.agenda.applications.Interfaces
{
    public interface IColaboradorApplicationService
    {
        Task<ResponseModel<PaginacaoDTO<ColaboradorDTO>>> BuscarUsuariosPaginado(PaginacaoDTO<UsuarioDTO> paginacao, string usuario);
        Task<ResponseModel<ColaboradorDTO>> BuscarUsuarioPorId(string id);
        Task<ResponseModel<ColaboradorDTO>> BuscarColaboladoresPorConta(string usuarioId);
        Task<ResponseModel<ColaboradorDTO>> Persistir(ColaboradorPayload usuarioPayload, string usuario);
        Task SalvarUsuarioConta(ColaboradorPayload usuarioPayload, string usuario, Colaborador colaboradorMap, UsuarioContaRegistradoEvent registradoEvent,
            UsuarioContaEvent contaEvent);
        Task<UsuarioRegistradoEvent> SalvarUsuario(ColaboradorPayload usuarioPayload);
    }
}
