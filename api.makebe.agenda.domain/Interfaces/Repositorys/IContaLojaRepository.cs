using api.makebe.agenda.domain.Entidades;

namespace api.makebe.agenda.infra.data.Repositorys.Interfaces
{
    public interface IContaLojaRepository
    {
        Task<int> Salvar(ContaLoja loja);
        Task<Loja> BuscarLojaPorCNPJ(string cnpj, Guid usuarioId);
    }
}
