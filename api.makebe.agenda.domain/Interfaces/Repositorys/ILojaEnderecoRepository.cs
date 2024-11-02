using api.makebe.agenda.domain.Entidades;

namespace api.makebe.agenda.infra.data.Repositorys
{
    public interface ILojaEnderecoRepository
    {
        Task<int> SalvarLojaEndereco(LojaEndereco endereco);
        Task<bool> AtualizaLojaEndereco(LojaEndereco endereco);
    }
}
