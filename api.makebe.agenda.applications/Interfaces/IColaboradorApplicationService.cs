using api.makebe.agenda.applications.Models.Payloads;
using api.makebe.agenda.applications.Models.Responses;
using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using api.makebesession.infra.crosscutting.Entidades;
using api.makebesession.infra.crosscutting.Events.Conta;
using api.makebesession.infra.crosscutting.Events.Usuarios;

namespace api.makebe.agenda.applications.Interfaces
{
    public interface IColaboradorApplicationService
    {
        Task<ResponseModel<PaginacaoDTO<ColaboradorDTO>>> BuscarUsuariosPaginado(PaginacaoDTO<UsuarioDTO> paginacao, string usuario);
        Task<ResponseModel<ColaboradorDTO>> BuscarUsuarioPorId(string id);

        Task<ResponseModel<ColaboradorDTO>> Persistir(ColaboradorPayload usuarioPayload, string usuario);
        Task SalvarUsuarioConta(ColaboradorPayload usuarioPayload, string usuario, Colaborador colaboradorMap, UsuarioContaRegistradoEvent registradoEvent,
            UsuarioContaEvent contaEvent);
        Task<UsuarioRegistradoEvent> SalvarUsuario(ColaboradorPayload usuarioPayload);
    }
}
