using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Interfaces.Repositorys;
using api.makebe.agenda.domain.Interfaces.Services;

namespace api.makebe.agenda.domain.Services
{
    public class LojaColaboradorDomainService : ILojaColaboradorDomainService
    {
        private readonly ILojaColaboradorRepository _lojaColaboradorRepository;
        public LojaColaboradorDomainService(ILojaColaboradorRepository lojaColaboradorRepository)
        {
            _lojaColaboradorRepository = lojaColaboradorRepository;
        }
        public async Task<IEnumerable<LojaColaboradorDTO>> BuscarColaboradorPorLoja(int lojaId)
        {
            var result = await _lojaColaboradorRepository.BuscarColaboradorPorLoja(lojaId);
            return result;
        }
        public async Task<int> Persistir(LojaColaborador colaborador)
        {
            colaborador.DataCadastro = DateTime.Now;
            colaborador.Status = true;
            if (colaborador.Id == 0)
            {
                return await _lojaColaboradorRepository.Salvar(colaborador);
            }
            await _lojaColaboradorRepository.Atualizar(colaborador);
            return colaborador.Id;
        }
    }
}
