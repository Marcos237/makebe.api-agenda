using api.makebe.agenda.domain.Entidades;

namespace api.makebe.agenda.applications.Interfaces
{
    public interface IEnderecoApplicationService
    {
        Task<bool> ValidarEnderecos(IEnumerable<Endereco> enderecos);
        Task<bool> SalvarEnderecos(IEnumerable<Endereco> enderecos);
        Task<IEnumerable<Endereco>> BuscarPorLojaId(int lojaId);
    }
}
