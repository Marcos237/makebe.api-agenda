using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;

namespace api.makebe.agenda.domain.Interfaces.Services
{
    public interface ILojaColaboradorDomainService
    {
        Task<IEnumerable<LojaColaboradorDTO>> BuscarColaboradorPorLoja(int lojaId);
        Task<int> Persistir(LojaColaborador colaborador);
    }
}
