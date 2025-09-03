using api.makebe.agenda.applications.Interfaces;
using api.makebe.agenda.applications.Models.Payloads;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Enums;
using api.makebe.agenda.domain.Interfaces.Services;
using AutoMapper;

namespace api.makebe.agenda.applications.Services.Portifolios
{
    public class PortifolioValidacaoAplicationService : IPortifolioValidacaoAplicationService
    {
        private readonly IValidationService<Portifolio> _validationPortifolioService;
        private readonly IValidationService<LojaPortifolio> _validationLojaService;
        private readonly IValidationService<ColaboradorPortifolio> _validationColaboradorService;
        private readonly IMapper _mapper;
        public PortifolioValidacaoAplicationService(IValidationService<Portifolio> validationPortifolioService, IValidationService<LojaPortifolio> validationLojaService,
            IValidationService<ColaboradorPortifolio> validationColaboradorService, IMapper mapper)
        {
            _validationPortifolioService = validationPortifolioService;
            _validationLojaService = validationLojaService;
            _validationColaboradorService = validationColaboradorService;
            _mapper = mapper;
        }
        public void RetornarListaVazia(string entidade, string mensagem)
        {
            _validationPortifolioService.RetornarListaVazia(entidade, mensagem);
        }

        public async Task<bool> Validar(PortifolioPayload portifolioPayload)
        {
            var portifolio = _mapper.Map<Portifolio>(portifolioPayload);
            var isValidate = await _validationPortifolioService.Validar(portifolio);

            if (portifolioPayload.TipoUsuarioId == (int)TipoUsuario.Loja)
            {
                var lojaMap = _mapper.Map<LojaPortifolio>(portifolioPayload);
                isValidate = await _validationLojaService.Validar(lojaMap);
                return isValidate;
            }
            var colaboradorMap = _mapper.Map<ColaboradorPortifolio>(portifolioPayload);
            isValidate = await _validationColaboradorService.Validar(colaboradorMap);
            return isValidate;
        }
    }
}
