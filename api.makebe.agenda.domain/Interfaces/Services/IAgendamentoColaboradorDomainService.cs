using api.makebe.agenda.domain.DTO;

namespace api.makebe.agenda.domain.Interfaces.Services
{
    public interface IAgendamentoColaboradorDomainService
    {
        Task<IEnumerable<ColaboradorDTO>> MontarColaboradores(IEnumerable<UsuarioDTO>? usuarios, string contaId);
    }
}
