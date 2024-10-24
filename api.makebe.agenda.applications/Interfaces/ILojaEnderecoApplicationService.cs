using api.makebe.agenda.domain.Entidades;

namespace api.makebe.agenda.applications.Interfaces
{
    public interface ILojaEnderecoApplicationService
    {
        Task<bool> SalvarLojaEndereco(LojaEndereco endereco);
    }
}
