using api.makebe.agenda.applications.Strategys.Interfaces.Enderecos;
using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Enums;
using api.makebe.agenda.domain.Helpers;
using api.makebe.agenda.domain.Interfaces.Services;
using api.makebe.agenda.infra.crosscutting.Services.Interfaces;
using AutoMapper;
using ContasEvent;

namespace api.makebe.agenda.applications.Strategys.Services.Enderecos
{
    public class ColaboradorEnderecoBuscaStrategy : IEnderecoBuscaStrategy
    {
        private readonly IColaboradorEnderecoDomainService _colaboradorEnderecoDomainService;
        private readonly IContaEventCrossCuttingService _contaEventCrossCuttingService;
        private readonly IMapper _mapper;
        public ColaboradorEnderecoBuscaStrategy(IColaboradorEnderecoDomainService colaboradorEnderecoDomainService, IContaEventCrossCuttingService contaEventCrossCuttingService,
        IMapper mapper)
        {
            _colaboradorEnderecoDomainService = colaboradorEnderecoDomainService;
            _contaEventCrossCuttingService = contaEventCrossCuttingService;
            _mapper = mapper;     
        }
        public async Task<PaginacaoDTO<EnderecoDTO>> BuscarEnderecos(PaginacaoDTO<EnderecoDTO> paginacao, string usuarioId)
        {
            if (paginacao.objetoPesquisa?.TipoUsuarioId == (int)TipoUsuario.Colaborador)
            {
                var conta = await _contaEventCrossCuttingService.BuscarContaPorId(PropiedadesHelper.ParseGuidOrDefault(usuarioId));
                var usuarioConsultadoEvent = new UsuarioContaConsultadoPorContaEvent() { IdConta = conta.Id ?? Guid.Empty };
                var usuariosConta = await _contaEventCrossCuttingService.BuscarUsuarioContaPorIdConta(usuarioConsultadoEvent);
                var usuariosMap = _mapper.Map<IEnumerable<UsuarioDTO>>(usuariosConta.UsuariosEvents);
                var response = await _colaboradorEnderecoDomainService.BuscarEndereco(paginacao, conta.Id.ToString() ?? string.Empty, usuariosMap);
                return response;
            }
            return paginacao;
        }
    }
}
