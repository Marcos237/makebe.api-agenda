using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Interfaces.Repositorys;
using api.makebe.agenda.domain.Interfaces.Services;

namespace api.makebe.agenda.domain.Services
{
    public class ColaboradorPortifolioDomainService : IColaboradorPortifolioDomainService
    {
        private readonly IPortifolioContextRepository<ColaboradorPortifolio, PortifolioDTO> _portifolioContextRepository;
        public ColaboradorPortifolioDomainService(IPortifolioContextRepository<ColaboradorPortifolio, PortifolioDTO> portifolioContextRepository)
        {
            _portifolioContextRepository = portifolioContextRepository;
        }
        public async Task<PaginacaoDTO<PortifolioDTO>> BuscarPortifolios(PaginacaoDTO<PortifolioDTO> paginacao, string contaId, IEnumerable<UsuarioDTO> usuarios)
        {
            var portifolios = await _portifolioContextRepository.BuscarPortifolios(contaId);
            paginacao.objetos = portifolios;
            var retornoPortifolio = await MontarColaborador(paginacao, usuarios);
            var portifolioFiltrado = await Filtrar(retornoPortifolio);
            return portifolioFiltrado;
        }
        public async Task<PaginacaoDTO<PortifolioDTO>> MontarColaborador(PaginacaoDTO<PortifolioDTO> paginacao, IEnumerable<UsuarioDTO> usuarios)
        {
            var portifoliosFiltrados = paginacao.objetos?.Join(usuarios, portifolio => portifolio.UsuarioId, usuario => usuario.Id,
                      (portifolio, usuario) => AdicionarPortifolio(portifolio, usuario));

            return await Task.FromResult(new PaginacaoDTO<PortifolioDTO>
            {
                paginaAtual = paginacao?.paginaAtual ?? 1,
                totalPaginas = paginacao?.totalPaginas ?? 1,
                quantidadePagina = paginacao?.quantidadePagina ?? 10,
                registroInicial = paginacao?.registroInicial ?? 1,
                objetoPesquisa = paginacao?.objetoPesquisa ?? new PortifolioDTO(),
                total = paginacao?.total ?? 0,
                objetos = portifoliosFiltrados?.ToList() ?? new List<PortifolioDTO>()
            });
        }

        public async Task<PaginacaoDTO<PortifolioDTO>> Filtrar(PaginacaoDTO<PortifolioDTO> paginacao)
        {
            paginacao.registroInicial = (paginacao.paginaAtual - 1) * paginacao.quantidadePagina;

            var filtrados = paginacao?.objetos?.Where(objeto =>
                 (string.IsNullOrEmpty(paginacao?.objetoPesquisa?.NomeColaborador) ||
                 objeto.NomeColaborador?.Contains(paginacao.objetoPesquisa.NomeColaborador, StringComparison.OrdinalIgnoreCase) == true) &&


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

        public async Task<int> Salvar(ColaboradorPortifolio item)
        {
            item.Status = true;
            if (item.Id == 0)
            {
                item.DataCadastro = DateTime.Now;
                var response = await _portifolioContextRepository.Salvar(item);
                return response;
            }
            await _portifolioContextRepository.Atualizar(item);
            return item.Id;
        }
        private static PortifolioDTO AdicionarPortifolio(PortifolioDTO portifolio, UsuarioDTO usuario)
        {
            return new PortifolioDTO
            {
                Id = portifolio.Id,
                ColaboradorId = portifolio.ColaboradorId,
                NomeColaborador = usuario.Nome,
                TipoUsuarioPortifolioId = portifolio.TipoUsuarioPortifolioId,
                Titulo = portifolio.Titulo,
                SubTitulo = portifolio.SubTitulo,
                Texto = portifolio.Texto,
                ContaId = portifolio.ContaId,


            };
        }

    }
}
