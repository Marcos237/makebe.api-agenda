using api.makebe.agenda.applications.Strategys.Interfaces.Enderecos;
using api.makebe.agenda.domain.DTO;

namespace api.makebe.agenda.applications.Strategys.Services.Enderecos
{
    public class EnderecoBuscaStrategyContext : IEnderecoBuscaStrategyContext
    {
        private readonly IEnumerable<IEnderecoBuscaStrategy> _enderecoBuscaStrategies;

        public EnderecoBuscaStrategyContext(IEnumerable<IEnderecoBuscaStrategy> enderecoBuscaStrategies)
        {
            _enderecoBuscaStrategies = enderecoBuscaStrategies;
        }
        public async Task<PaginacaoDTO<EnderecoDTO>> Buscar(PaginacaoDTO<EnderecoDTO> paginacao, string contaId)
        {

            foreach (var strategy in _enderecoBuscaStrategies)
            {
                paginacao = await strategy.BuscarEnderecos(paginacao, contaId);
            }
            return paginacao;
        }
    }
}
