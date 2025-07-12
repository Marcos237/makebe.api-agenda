using api.makebe.agenda.infra.crosscutting.Entidades;

namespace api.makebe.agenda.infra.crosscutting.Services.Interfaces
{
    public interface IUsuarioClienteConsultadosCrosCuttingService
    {
        Task<IEnumerable<UsuarioEvent>> BuscarUsuarioClientes();
    }
}
