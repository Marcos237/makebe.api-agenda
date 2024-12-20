using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;

namespace api.makebe.agenda.domain.Interfaces.Repositorys
{
    public interface ILojaColaboradorRepository
    {
        Task<IEnumerable<LojaColaboradorDTO>> BuscarColaboradorPorLoja(int lojaId);
        Task<int> Salvar(LojaColaborador colaborador);
        Task<LojaColaborador> Atualizar(LojaColaborador colaborador);
    }
}
