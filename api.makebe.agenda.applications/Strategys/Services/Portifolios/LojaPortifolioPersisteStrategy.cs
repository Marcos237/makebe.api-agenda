using api.makebe.agenda.applications.Models.Payloads;
using api.makebe.agenda.applications.Strategys.Interfaces;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Enums;
using api.makebe.agenda.domain.Interfaces.Services;
using AutoMapper;

namespace api.makebe.agenda.applications.Strategys.Services.Portifolios
{
    public class LojaPortifolioPersisteStrategy : IPortifolioPersisteStrategy<PortifolioPayload>
    {
        private readonly ILojaPortifolioDomainService _lojaPortifolioDomainService;
        private readonly IMapper _mapper;

        public LojaPortifolioPersisteStrategy(ILojaPortifolioDomainService lojaPortifolioDomainService, IMapper mapper)
        {
            _lojaPortifolioDomainService = lojaPortifolioDomainService;
            _mapper = mapper;
        }
        public async Task<int> Salvar(PortifolioPayload item)
        {
            var itemNaoSalvo = 0;
            if (item.TipoUsuarioPortifolioId == (int)TipoUsuarioPortifolio.Loja)
            {
                var lojaMap = _mapper.Map<LojaPortifolio>(item);
                var response = await _lojaPortifolioDomainService.Salvar(lojaMap);
                return response == 0 ? 0 : response;
            }
            return itemNaoSalvo;
        }
    }
}