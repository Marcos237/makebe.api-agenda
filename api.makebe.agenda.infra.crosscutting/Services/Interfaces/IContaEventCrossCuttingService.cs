using api.makebe.agenda.infra.crosscutting.Entidades;
using ContasEvent;

namespace api.makebe.agenda.infra.crosscutting.Services.Interfaces
{
    public interface IContaEventCrossCuttingService
    {
        Task<ContaEvent> BuscarContaPorId(Guid usuarioId);
        Task<UsuarioContaConsultadoPorContaEvent> BuscarUsuarioContaPorIdConta(UsuarioContaConsultadoPorContaEvent usuarioContaConsultadoPorContaEvent);
        Task DeletarContaUsuario(UsuarioContaDeletadoEvent usuarioContaDeletadoEvent);
        Task<UsuarioContaRegistradoEvent> SalvarUsuarioConta(UsuarioContaRegistradoEvent usuarioContaRegistradoEvent);
    }
}
