using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Enum;

namespace api.makebe.agenda.applications.Interfaces
{
    public interface IPermissaoAutenticacaoService
    {
        Task<IEnumerable<Papeis>> ValidarPermissaoAutenticacao(PapeisPermissoes papeisPermissoes, string papeis, string chave);

        string PermissaoEnumParaTexto(PapeisPermissoes papeisPermissoes);
    }
}
