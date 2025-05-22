using api.makebe.agenda.applications.Interfaces;
using api.makebe.agenda.applications.Models.Payloads;
using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Helpers;
using api.makebe.agenda.domain.Interfaces.Services;
using api.makebe.agenda.infra.crosscutting.Services.Interfaces;
using AutoMapper;
using ContasEvent;

namespace api.makebe.agenda.applications.Services
{
    public class EnderecoColaboradorApplicationService : IEnderecoContextApplicationService
    {
        private readonly IColaboradorEnderecoDomainService _colaboradorEnderecoDomainService;
        private readonly IContaEventCrossCuttingService _contaEventCrossCuttingService;
        private readonly IMapper _mapper;

        public EnderecoColaboradorApplicationService(IColaboradorEnderecoDomainService colaboradorEnderecoDomainService, IContaEventCrossCuttingService contaEventCrossCuttingService,
        IMapper mapper)
        {
            _colaboradorEnderecoDomainService = colaboradorEnderecoDomainService;
            _contaEventCrossCuttingService = contaEventCrossCuttingService;
            _mapper = mapper;
        }
        public async Task<PaginacaoDTO<EnderecoDTO>> BuscarEnderecos(PaginacaoDTO<EnderecoDTO> paginacao, string usuarioId)
        {
            var conta = await _contaEventCrossCuttingService.BuscarContaPorId(PropiedadesHelper.ParseGuidOrDefault(usuarioId));
            var usuarioConsultadoEvent = new UsuarioContaConsultadoPorContaEvent() { IdConta = conta.Id ?? Guid.Empty };
            var usuariosConta = await _contaEventCrossCuttingService.BuscarUsuarioContaPorIdConta(usuarioConsultadoEvent);
            var usuariosMap = _mapper.Map<IEnumerable<UsuarioDTO>>(usuariosConta.UsuariosEvents);
            var response = await _colaboradorEnderecoDomainService.BuscarEndereco(paginacao, conta.Id.ToString() ?? string.Empty, usuariosMap);
            return response;
        }

        public async Task<int> Salvar(EnderecoPayload item)
        {
            var colaboradorMap = _mapper.Map<ColaboradorEndereco>(item);
            var response = await _colaboradorEnderecoDomainService.Salvar(colaboradorMap);
            return response == 0 ? 0 : response;
        }
    }
}
