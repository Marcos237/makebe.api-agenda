using api.makebe.agenda.applications.Strategys.Interfaces.Enderecos;
using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Enums;
using api.makebe.agenda.domain.Helpers;
using api.makebe.agenda.domain.Interfaces.Services;
using api.makebe.agenda.infra.crosscutting.Services.Interfaces;

namespace api.makebe.agenda.applications.Strategys.Services.Enderecos
{
    public class LojaEnderecoBuscaStrategy : IEnderecoBuscaStrategy
    {
        private readonly ILojaEnderecoDomainService _lojaEnderecoDomainService;
        private readonly IContaEventCrossCuttingService _contaEventCrossCuttingService;
        public LojaEnderecoBuscaStrategy(ILojaEnderecoDomainService lojaEnderecoDomainService, IContaEventCrossCuttingService contaEventCrossCuttingService)
        {
            _lojaEnderecoDomainService = lojaEnderecoDomainService;
            _contaEventCrossCuttingService = contaEventCrossCuttingService;
        }
        public async Task<PaginacaoDTO<EnderecoDTO>> BuscarEnderecos(PaginacaoDTO<EnderecoDTO> paginacao, string usuarioId)
        {
            if (paginacao.objetoPesquisa?.TipoUsuarioId == (int)TipoUsuario.Loja)
            {
                var conta = await _contaEventCrossCuttingService.BuscarContaPorId(PropiedadesHelper.ParseGuidOrDefault(usuarioId));
                var response = await _lojaEnderecoDomainService.BuscarEnderecos(paginacao, conta.Id.ToString() ?? string.Empty);
                return response;
            }
            return paginacao;
        }
    }
}
