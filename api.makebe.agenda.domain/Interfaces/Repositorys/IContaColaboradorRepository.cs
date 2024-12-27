using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;

namespace api.makebe.agenda.applications.Interfaces
{
    public interface IContaColaboradorRepository
    {
        Task<IEnumerable<ColaboradorDTO>> BuscarColaboradorPorContaId(string contaId);
        Task<int> Salvar(ContaColaborador colaborador);
        Task<bool> Atualizar(ContaColaborador colaborador);
    }
}
