using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Interfaces.Repositorys;
using api.makebe.agenda.domain.Interfaces.Services;

namespace api.makebe.agenda.domain.Services
{
    public class LojaPortifolioDomainService : ILojaPortifolioDomainService
    {
        private readonly IPortifolioContextRepository<LojaPortifolio, PortifolioDTO> _portifolioContextRepository;
        public LojaPortifolioDomainService(IPortifolioContextRepository<LojaPortifolio, PortifolioDTO> portifolioContextRepository)
        {
            _portifolioContextRepository = portifolioContextRepository;
        }

        public async Task<PaginacaoDTO<PortifolioDTO>> BuscarPortifolios(PaginacaoDTO<PortifolioDTO> paginacao, string contaId)
        {
            var response = await _portifolioContextRepository.BuscarPortifolios(contaId);  
            paginacao.objetos = response;
            var filtrados = await Filtrar(paginacao);
            return filtrados;
        }
        public async Task<PaginacaoDTO<PortifolioDTO>> Filtrar(PaginacaoDTO<PortifolioDTO> paginacao)
        {
            paginacao.registroInicial = (paginacao.paginaAtual - 1) * paginacao.quantidadePagina;

            var filtrados = paginacao?.objetos?.Where(objeto =>
                 (string.IsNullOrEmpty(paginacao?.objetoPesquisa?.RazaoSocial) ||
                 objeto.RazaoSocial?.Contains(paginacao.objetoPesquisa.RazaoSocial, StringComparison.OrdinalIgnoreCase) == true) &&


                (string.IsNullOrEmpty(paginacao?.objetoPesquisa?.Titulo) ||
                 objeto.Titulo?.Contains(paginacao.objetoPesquisa.Titulo, StringComparison.OrdinalIgnoreCase) == true) &&

                (string.IsNullOrEmpty(paginacao?.objetoPesquisa?.SubTitulo) ||
                 objeto.SubTitulo?.Contains(paginacao.objetoPesquisa.SubTitulo, StringComparison.OrdinalIgnoreCase) == true)

            ) ?? Enumerable.Empty<PortifolioDTO>();
            paginacao!.total = filtrados?.Count() ?? 0;
            paginacao!.totalPaginas = (paginacao.total + paginacao.quantidadePagina - 1) / paginacao.quantidadePagina;

            paginacao.objetos = filtrados?.Skip(paginacao.registroInicial).Take(paginacao.quantidadePagina);
            return await Task.FromResult(paginacao);
        }
        public async Task<int> Salvar(LojaPortifolio item)
        {
            item.Status = true;
            if(item.Id == 0) {
                item.DataCadastro = DateTime.Now;
                var retorno = await _portifolioContextRepository.Salvar(item);
                return retorno;
            }
            await _portifolioContextRepository.Atualizar(item);
            return item.Id;
        }
    }
}
