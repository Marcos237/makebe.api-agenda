using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Interfaces.Repositorys;
using api.makebe.agenda.domain.Interfaces.Services;

namespace api.makebe.agenda.domain.Services
{
    public class TipoLojaDomainService : ITipoLojaDomainService
    {
        private readonly ITipoLojaRepository _tipoLojaRepository;
        public TipoLojaDomainService(ITipoLojaRepository tipoLojaRepository)
        {
            _tipoLojaRepository = tipoLojaRepository;
        }
        public async Task<IEnumerable<TipoLoja>> BuscarTodos()
        {
            var retorno = await _tipoLojaRepository.BuscarTodos();
            return retorno;
        }
    }
}
