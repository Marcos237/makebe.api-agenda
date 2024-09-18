using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.infra.data.Repositorys.Interfaces;

namespace api.makebe.agenda.domain.Services
{
    public class UsuarioLojaDomainService : IUsuarioLojaDomainService
    {
        private readonly IUsuarioLojaRepository _usuarioLojaRepository;
        public UsuarioLojaDomainService(IUsuarioLojaRepository usuarioLojaRepository)
        {
            _usuarioLojaRepository = usuarioLojaRepository;
        }
        public Task<int> Salvar(UsuarioLoja loja)
        {
            loja.DataCadastro = DateTime.Now;
            loja.Status = true;
            var retorno = _usuarioLojaRepository.Salvar(loja);
            return retorno;
        }
    }
}
