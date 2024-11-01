using api.makebe.agenda.applications.Interfaces;
using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Interfaces.Services;
using AutoMapper;

namespace api.makebe.agenda.applications.Services
{
    public class LojaPortifolioImagensApplicationService : ILojaPortifolioImagemApplicationService
    {
        private readonly IValidationService<Arquivo> _validationArquivoService;
        private readonly IMapper _mapper;
        private readonly ILojaPortifolioImagemDomainService _lojaImagemDomainService;
        private readonly IArquivoDomainService _arquivoDomainService;
        public LojaPortifolioImagensApplicationService(IValidationService<Arquivo> validationService, IMapper mapper, ILojaPortifolioImagemDomainService lojaImagemDomainService, IArquivoDomainService arquivoDomainService)
        {
            _validationArquivoService = validationService;
            _mapper = mapper;
            _lojaImagemDomainService = lojaImagemDomainService;
            _arquivoDomainService = arquivoDomainService;
        }

        public async Task<IEnumerable<LojaPortifolioImagemDTO>> SalvarImagens(IEnumerable<LojaPortifolioImagemDTO> lojaPortifolioImagens)
        {
            var id = lojaPortifolioImagens.FirstOrDefault()?.LojaPortifolioId ?? 0;
            await _lojaImagemDomainService.Desativar(id);
            foreach (var imagem in lojaPortifolioImagens)
            {
                var arquivo = _arquivoDomainService.MontarArquivo(imagem.UrlImagem ?? string.Empty, imagem?.NomeImagem ?? string.Empty);
                var imagemMap = _mapper.Map<LojaPortifolioImagens>(arquivo);
                await _lojaImagemDomainService.Salvar(imagemMap);
            }
            var retornoImegame = await _lojaImagemDomainService.BuscarImagensPorIdLojaPortifolio(id);
            return retornoImegame;

        }

        public async Task<bool> ValidarArquivos(IEnumerable<Arquivo> arquivos)
        {
            if (arquivos.Any())
            {
                foreach (var arquivo in arquivos)
                {
                    var isvalid = await _validationArquivoService.Validar(arquivo);
                    if (!isvalid)
                        return false;
                }
            }
            return true;
        }

    }
}
