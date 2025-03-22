using api.makebe.agenda.applications.Interfaces;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Interfaces.Repositorys;
using api.makebe.agenda.domain.Interfaces.Services;

namespace api.makebe.agenda.domain.Services
{
    public class ContaServicoDomainService : IContaServicoDomainService
    {
        private readonly IContaServicoRepository _contaServicoRepository;
        public ContaServicoDomainService(IContaServicoRepository contaServicoRepository)
        {
            _contaServicoRepository = contaServicoRepository;
        }
        public async Task<IEnumerable<ContaServico>> BuscarServicoPorConta(string contaId)
        {
            var response = await _contaServicoRepository.BuscarServicoPorConta(contaId);

            return response;
        }

        public async Task<int> Salvar(ContaServico contaServico, int id)
        {
            if(id > 0)
            {
                await _contaServicoRepository.Atualizar(contaServico);
                return id;
            }
            contaServico.DataCadastro = DateTime.Now;
            var idContaServico = await _contaServicoRepository.Salvar(contaServico);
            return idContaServico;
        }
    }
}
