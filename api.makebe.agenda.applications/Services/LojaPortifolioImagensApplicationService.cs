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

        public async Task<IEnumerable<LojaPortifolioImagemDTO>> BuscarImagensPorLojaPortifolioId(int lojaPortifolioId)
        {
            var retorno = await _lojaImagemDomainService.BuscarImagensPorIdLojaPortifolio(lojaPortifolioId);
            return retorno;
        }

        public async Task<bool> SalvarImagens(IEnumerable<LojaPortifolioImagemDTO> lojaPortifolioImagens, int lojaPortifolioId)
        {
            await _lojaImagemDomainService.Desativar(lojaPortifolioId);
            var retorno = false;
            foreach (var imagem in lojaPortifolioImagens)
            {
                var arquivo = await _arquivoDomainService.MontarArquivo(imagem.UrlImagem ?? string.Empty, imagem?.NomeImagem ?? string.Empty);
                arquivo.TituloImagem = imagem?.TituloImagem;
                var imagemMap = _mapper.Map<LojaPortifolioImagens>(arquivo);
                imagemMap.LojaPortifolioId = lojaPortifolioId;
                retorno = await _lojaImagemDomainService.Salvar(imagemMap) > 0;
            }
            return retorno;
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
