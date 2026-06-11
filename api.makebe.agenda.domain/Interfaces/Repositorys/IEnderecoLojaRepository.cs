using api.makebe.agenda.domain.DTO;

namespace api.makebe.agenda.domain.Interfaces.Repositorys
{
    public interface IEnderecoLojaRepository
    {
        Task<IEnumerable<EnderecoLojaDTO>> BuscarEnderecoLoja(int id);
    }
}
