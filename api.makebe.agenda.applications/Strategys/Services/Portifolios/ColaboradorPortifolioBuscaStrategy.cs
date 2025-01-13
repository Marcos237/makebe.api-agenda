using api.makebe.agenda.applications.Strategys.Interfaces;
using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Enums;
using api.makebe.agenda.domain.Helpers;
using api.makebe.agenda.domain.Interfaces.Services;
using api.makebe.agenda.infra.crosscutting.Services.Interfaces;
using AutoMapper;
using ContasEvent;

namespace api.makebe.agenda.applications.Strategys.Services.Portifolios
{
    public class ColaboradorPortifolioBuscaStrategy : IPortifolioBuscaStrategy
    {
        private readonly IColaboradorPortifolioDomainService _colaboradorPortifolioDomainService;
        private readonly IContaEventCrossCuttingService _contaEventCrossCuttingService;
        private readonly IMapper _mapper;

        public ColaboradorPortifolioBuscaStrategy(IColaboradorPortifolioDomainService colaboradorPortifolioDomainService, IContaEventCrossCuttingService contaEventCrossCuttingService,
            IMapper mapper)
        {
            _colaboradorPortifolioDomainService = colaboradorPortifolioDomainService;
            _contaEventCrossCuttingService = contaEventCrossCuttingService;
            _mapper = mapper;
        }
        public async Task<PaginacaoDTO<PortifolioDTO>> BuscarPortifolios(PaginacaoDTO<PortifolioDTO> paginacao, string usuarioId)
        {
            if (paginacao.objetoPesquisa?.TipoUsuarioPortifolioId == (int)TipoUsuarioPortifolio.Colaborador)
            {
                var conta = await _contaEventCrossCuttingService.BuscarContaPorId(PropiedadesHelper.ParseGuidOrDefault(usuarioId));
                var usuarioConsultadoEvent = new UsuarioContaConsultadoPorContaEvent() { IdConta = conta.Id ?? Guid.Empty };
                var usuariosConta = await _contaEventCrossCuttingService.BuscarUsuarioContaPorIdConta(usuarioConsultadoEvent);
                var usuariosMap = _mapper.Map<IEnumerable<UsuarioDTO>>(usuariosConta.UsuariosEvents);


                var response = await _colaboradorPortifolioDomainService.BuscarPortifolios(paginacao,  conta.Id.ToString() ?? string.Empty, usuariosMap);
                return response;
            }
            return paginacao;
        }
    }
}
