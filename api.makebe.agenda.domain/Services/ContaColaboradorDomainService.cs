using api.makebe.agenda.applications.Interfaces;
using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Interfaces.Services;

namespace api.makebe.agenda.domain.Services
{
    public class ContaColaboradorDomainService : IContaColaboradorDomainService
    {
        private readonly IContaColaboradorRepository _contaColaboradorRepository;
        public ContaColaboradorDomainService(IContaColaboradorRepository contaColaboradorRepository)
        {
            _contaColaboradorRepository = contaColaboradorRepository;
        }
        public async Task<IEnumerable<ColaboradorDTO>> BuscarColaboradorPorUsuarioId(string usuarioId)
        {
            var colaboradores = await  _contaColaboradorRepository.BuscarColaboradorPorContaId(usuarioId);
            return colaboradores;

        }
        public async Task<int> Salvar(ContaColaborador colaborador)
        {
            colaborador.DataCadastro = DateTime.Now;
            var idColaborador = await _contaColaboradorRepository.Salvar(colaborador);
            return idColaborador;
        }
    }
}
