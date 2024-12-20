using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.infra.data.Repositorys.Interfaces;

namespace api.makebe.agenda.domain.Services
{
    public class ContaLojaDomainService : IContaLojaDomainService
    {
        private readonly IContaLojaRepository _contaLojaRepository;
        public ContaLojaDomainService(IContaLojaRepository contaLojaRepository)
        {
            _contaLojaRepository = contaLojaRepository;
        }
        public Task<int> Salvar(ContaLoja loja)
        {
            loja.DataCadastro = DateTime.Now;
            loja.Status = true;
            var retorno = _contaLojaRepository.Salvar(loja);
            return retorno;
        }
    }
}
