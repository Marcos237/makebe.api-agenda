using api.makebe.agenda.applications.Interfaces;
using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Interfaces.Services;
using AutoMapper;

namespace api.makebe.agenda.applications.Services.Portifolios
{
    public class PortifolioImagensApplicationService : IPortifolioImagemApplicationService
    {
        private readonly IValidationService<Arquivo> _validationArquivoService;
        private readonly IMapper _mapper;
        private readonly IPortifolioImagemDomainService _imagemDomainService;
        private readonly IArquivoDomainService _arquivoDomainService;
        public PortifolioImagensApplicationService(IValidationService<Arquivo> validationService, IMapper mapper, IPortifolioImagemDomainService lojaImagemDomainService, IArquivoDomainService arquivoDomainService)
        {
            _validationArquivoService = validationService;
            _mapper = mapper;
            _imagemDomainService = lojaImagemDomainService;
            _arquivoDomainService = arquivoDomainService;
        }

        public async Task<IEnumerable<PortifolioImagemDTO>> BuscarImagensPorLojaPortifolioId(int lojaPortifolioId)
        {
            var retorno = await _imagemDomainService.BuscarImagensPorIdPortifolio(lojaPortifolioId);
            return retorno;
        }

        public async Task<bool> SalvarImagens(IEnumerable<PortifolioImagemDTO> lojaPortifolioImagens, int lojaPortifolioId)
        {
            await _imagemDomainService.Desativar(lojaPortifolioId);
            var retorno = false;
            foreach (var imagem in lojaPortifolioImagens)
            {
                var arquivo = await _arquivoDomainService.MontarArquivo(imagem.UrlImagem ?? string.Empty, imagem?.NomeImagem ?? string.Empty);
                arquivo.TituloImagem = imagem?.TituloImagem;
                var imagemMap = _mapper.Map<PortifolioImagens>(arquivo);
                imagemMap.PortifolioId = lojaPortifolioId;
                retorno = await _imagemDomainService.Salvar(imagemMap) > 0;
            }
            return retorno;
        }

        public async Task<bool> ValidarArquivos(IEnumerable<Arquivo> arquivos)
        {
            var erro = 0;
            var countErros = new List<int>();
            if (arquivos.Any())
            {
                foreach (var arquivo in arquivos)
                {
                    var isvalid = await _validationArquivoService.Validar(arquivo);
                    if (!isvalid)
                        countErros.Add(erro + 1);
                }
            }
            if (countErros.Any())
                return false;

            return true;
        }
    }
}
