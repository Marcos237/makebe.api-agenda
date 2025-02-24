using api.makebe.agenda.applications.Models.Payloads;
using api.makebe.agenda.applications.Strategys.Interfaces.Portifolios;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Enums;
using api.makebe.agenda.domain.Interfaces.Services;
using AutoMapper;

namespace api.makebe.agenda.applications.Strategys.Services.Portifolios
{
    public class ColaboradorPortifolioPersiteStrategy : IPortifolioPersisteStrategy<PortifolioPayload>
    {
        private readonly IColaboradorPortifolioDomainService _colaboradorPortifolioDomainService;
        private readonly IMapper _mapper;

        public ColaboradorPortifolioPersiteStrategy(IColaboradorPortifolioDomainService colaboradorPortifolioDomainService, IMapper mapper)
        {
            _colaboradorPortifolioDomainService = colaboradorPortifolioDomainService;
            _mapper = mapper;
        }
        public async Task<int> Salvar(PortifolioPayload item)
        {
            var itemNaoSalvo = 0;
            if (item.TipoUsuarioId == (int)TipoUsuario.Colaborador)
            {
                var colaboradorMap = _mapper.Map<ColaboradorPortifolio>(item);
                var response = await _colaboradorPortifolioDomainService.Salvar(colaboradorMap);
                return response == 0 ? 0 : response;
            }
            return itemNaoSalvo;
        }
    }
}