using UsuariosEvent;

namespace api.makebe.agenda.infra.crosscutting.Services.Interfaces
{
    public interface IUsuarioEventCrossCuttingService
    {
        Task<UsuarioConsultadoPorIdEvent> BuscarUsuarioPorId(UsuarioConsultadoPorIdEvent usuarioConsultadoPorIdEvent);
        Task<UsuariosConsutadosPorIdsEvent> BuscarUsuariosPorIds(UsuariosConsutadosPorIdsEvent usuariosConsultadosPorIdEvent);
        Task DeletarUsuario(UsuarioDeletadoEvent usuarioDeletadoEvent);
        Task<UsuarioRegistradoEvent> SalvarUsuario(UsuarioRegistradoEvent usuarioRegistradoEvent);
        Task<UsuariosPaginadoEvent> BuscarPaginado(UsuariosPaginadoEvent usuariosPaginadoEvent);
    }
}
