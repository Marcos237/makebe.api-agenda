using api.makebe.agenda.domain.Entidades;

namespace api.makebe.agenda.domain.Interfaces.Services
{
    public interface ILojaEnderecoDomainService
    {
        Task<int> SalvarLojaEndereco(LojaEndereco endereco);
    }
}
