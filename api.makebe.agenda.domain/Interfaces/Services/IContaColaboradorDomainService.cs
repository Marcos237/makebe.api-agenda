using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;

namespace api.makebe.agenda.domain.Interfaces.Services
{
    public interface IContaColaboradorDomainService
    {
        Task<IEnumerable<ColaboradorDTO>> BuscarColaboradorPorUsuarioId(string usuarioId);
        Task<int> Salvar(ContaColaborador colaborador);
    }
}
