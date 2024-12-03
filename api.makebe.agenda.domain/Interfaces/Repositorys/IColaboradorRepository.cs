using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;

namespace api.makebe.agenda.domain.Interfaces.Repositorys
{
    public interface IColaboradorRepository
    {
        Task<LojaColaboradorDTO> BuscarPorId(int id);
        Task<int> Salvar(Colaborador colaborador);
        Task<Colaborador> Atualizar(Colaborador colaborador);
        Task<bool> Desativar(int id);
    }
}
