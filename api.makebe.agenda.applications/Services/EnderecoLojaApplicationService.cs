using api.makebe.agenda.applications.Interfaces;
using api.makebe.agenda.applications.Models.Payloads;
using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Helpers;
using api.makebe.agenda.domain.Interfaces.Services;
using api.makebe.agenda.domain.Services;
using api.makebe.agenda.infra.crosscutting.Services.Interfaces;
using AutoMapper;

namespace api.makebe.agenda.applications.Services
{
    public class EnderecoLojaApplicationService : IEnderecoContextApplicationService
    {

        private readonly ILojaEnderecoDomainService _lojaEnderecoDomainService;
        private readonly IContaEventCrossCuttingService _contaEventCrossCuttingService;
        private readonly IMapper _mapper;

        public EnderecoLojaApplicationService(ILojaEnderecoDomainService lojaEnderecoDomainService, IContaEventCrossCuttingService contaEventCrossCuttingService, IMapper mapper)
        {
            _lojaEnderecoDomainService = lojaEnderecoDomainService;
            _contaEventCrossCuttingService = contaEventCrossCuttingService;
            _mapper = mapper;
        }
        public async Task<PaginacaoDTO<EnderecoDTO>> BuscarEnderecos(PaginacaoDTO<EnderecoDTO> paginacao, string usuarioId)
        {
            var conta = await _contaEventCrossCuttingService.BuscarContaPorId(PropiedadesHelper.ParseGuidOrDefault(usuarioId));
            var response = await _lojaEnderecoDomainService.BuscarEnderecos(paginacao, conta.Id.ToString() ?? string.Empty);
            return response;
        }

        public async Task<int> Salvar(EnderecoPayload item)
        {
            var lojaMap = _mapper.Map<LojaEndereco>(item);
            var response = await _lojaEnderecoDomainService.Salvar(lojaMap);
            return response == 0 ? 0 : response;
        }
    }
}
