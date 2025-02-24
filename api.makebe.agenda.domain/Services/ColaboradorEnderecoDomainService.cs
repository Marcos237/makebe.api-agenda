using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Interfaces.Services;
using api.makebe.agenda.infra.data.Repositorys;

namespace api.makebe.agenda.domain.Services
{
    public class ColaboradorEnderecoDomainService : IColaboradorEnderecoDomainService
    {
        private readonly IEnderecoContextRepository<ColaboradorEndereco, EnderecoDTO> _enderecoContextRepository;
        public ColaboradorEnderecoDomainService(IEnderecoContextRepository<ColaboradorEndereco, EnderecoDTO> enderecoContextRepository)
        {
            _enderecoContextRepository = enderecoContextRepository;
        }
        public async Task<PaginacaoDTO<EnderecoDTO>> BuscarEndereco(PaginacaoDTO<EnderecoDTO> paginacao, string contaId, IEnumerable<UsuarioDTO> usuarios)
        {
            var enderecos = await _enderecoContextRepository.BuscarEnderecos(contaId);
            paginacao.objetos = enderecos;
            var retornoEndereco = await MontarColaborador(paginacao, usuarios);
            var enderecoFiltrado = await Filtrar(retornoEndereco);
            return enderecoFiltrado;
        }

        public async Task<PaginacaoDTO<EnderecoDTO>> MontarColaborador(PaginacaoDTO<EnderecoDTO> paginacao, IEnumerable<UsuarioDTO> usuarios)
        {
            var portifoliosFiltrados = paginacao.objetos?.Join(usuarios, endereco => endereco.UsuarioId, usuario => usuario.Id,
          (endereco, usuario) => AdicionarEndereco(endereco, usuario));

            return await Task.FromResult(new PaginacaoDTO<EnderecoDTO>
            {
                paginaAtual = paginacao?.paginaAtual ?? 1,
                totalPaginas = paginacao?.totalPaginas ?? 1,
                quantidadePagina = paginacao?.quantidadePagina ?? 10,
                registroInicial = paginacao?.registroInicial ?? 1,
                objetoPesquisa = paginacao?.objetoPesquisa ?? new EnderecoDTO(),
                total = paginacao?.total ?? 0,
                objetos = portifoliosFiltrados?.ToList() ?? new List<EnderecoDTO>()
            });
        }
        public async Task<PaginacaoDTO<EnderecoDTO>> Filtrar(PaginacaoDTO<EnderecoDTO> paginacao)
        {
            paginacao.registroInicial = (paginacao.paginaAtual - 1) * paginacao.quantidadePagina;

            var filtrados = paginacao?.objetos?.Where(objeto =>
                 (string.IsNullOrEmpty(paginacao?.objetoPesquisa?.NomeColaborador) ||
                 objeto.NomeColaborador?.Contains(paginacao.objetoPesquisa.NomeColaborador, StringComparison.OrdinalIgnoreCase) == true) &&

                (string.IsNullOrEmpty(paginacao?.objetoPesquisa?.Logradouro) ||
                 objeto.Logradouro?.Contains(paginacao.objetoPesquisa.Logradouro, StringComparison.OrdinalIgnoreCase) == true))
                ?? Enumerable.Empty<EnderecoDTO>();

            paginacao!.total = filtrados?.Count() ?? 0;
            paginacao!.totalPaginas = (paginacao.total + paginacao.quantidadePagina - 1) / paginacao.quantidadePagina;

            paginacao.objetos = filtrados?.Skip(paginacao.registroInicial).Take(paginacao.quantidadePagina);
            return await Task.FromResult(paginacao);
        }

        public async Task<int> Salvar(ColaboradorEndereco item)
        {
            item.Status = true;
            if (item.Id == 0)
            {
                item.DataCadastro = DateTime.Now;
                var response = await _enderecoContextRepository.Salvar(item);
                return response;
            }
            await _enderecoContextRepository.Atualizar(item);
            return item.Id;
        }
        private static EnderecoDTO AdicionarEndereco(EnderecoDTO endereco, UsuarioDTO usuario)
        {
            return new EnderecoDTO
            {
                Id = endereco.Id,
                ColaboradorId = endereco.ColaboradorId,
                NomeColaborador = usuario.Nome,
                TipoUsuarioId = endereco.TipoUsuarioId,
                Logradouro = endereco.Logradouro,
                Numero = endereco.Numero,
                CEP = endereco.CEP,
                Estado = endereco.Estado,
                Cidade = endereco.Cidade,
                Complemento = endereco.Complemento,
                ColaboradorEnderecoId = endereco.ColaboradorEnderecoId
            };
        }
    }
}
