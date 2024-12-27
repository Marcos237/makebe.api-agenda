using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Interfaces.Repositorys;
using api.makebe.agenda.domain.Interfaces.Services;

namespace api.makebe.agenda.domain.Services
{
    public class ColaboradorProfissionalDomainService : IColaboradorProfissionalDomainService
    {
        private readonly IColaboradorProfissionalRepository _ColaboradorProfissionalRepository;
        public ColaboradorProfissionalDomainService(IColaboradorProfissionalRepository ColaboradorProfissionalRepository)
        {
            _ColaboradorProfissionalRepository = ColaboradorProfissionalRepository;
        }
        public async Task<PaginacaoDTO<ColaboradorProfissionalDTO>> BuscarPaginado(PaginacaoDTO<ColaboradorProfissionalDTO> paginacao, string contaId, IEnumerable<UsuarioDTO> usuarios)
        {
            var colaboradores = await _ColaboradorProfissionalRepository.BuscarPorContaId(contaId) ?? Enumerable.Empty<ColaboradorProfissionalDTO>();
            paginacao.objetos = colaboradores;
            var retornoColaborador = await MontarColaboradorProfissional(paginacao, usuarios);
            var colaboradorFiltrado = await Filtrar(retornoColaborador);
            return colaboradorFiltrado;
        }
        public async Task<ColaboradorProfissionalDTO> BuscarPorId(int id)
        {
            var retorno = await _ColaboradorProfissionalRepository.BuscarPorId(id);
            return retorno;
        }
        public async Task<int> Salvar(ColaboradorProfissional colaborador)
        {
            if (colaborador.Id == 0)
            {
                colaborador.DataAtualizacao = DateTime.Now;
                colaborador.DataCadastro = DateTime.Now;
                colaborador.Status = true;
                var retornoSalvar = await _ColaboradorProfissionalRepository.Salvar(colaborador);
                return retornoSalvar;
            }
            colaborador.DataAtualizacao = DateTime.Now;
            var retorno = await _ColaboradorProfissionalRepository.Atualizar(colaborador);
            return colaborador.Id;
        }

        public async Task<bool> Desativar(int id)
        {
            return await _ColaboradorProfissionalRepository.Desativar(id);
        }

        public async Task<IEnumerable<string>> MontarIdsPesquisas(IEnumerable<ColaboradorProfissionalDTO> colaboradores)
        {
            var retorno = colaboradores
                .Select(colaborador => colaborador.UsuarioId?.ToString() ?? string.Empty).Where(id => !string.IsNullOrEmpty(id));

            return await Task.FromResult(retorno);
        }
        public async Task<PaginacaoDTO<ColaboradorProfissionalDTO>> MontarColaboradorProfissional(PaginacaoDTO<ColaboradorProfissionalDTO> paginacao, IEnumerable<UsuarioDTO> usuarios)
        {
            var colaboradoresFiltrados = paginacao.objetos?.Join(usuarios, colaborador => colaborador.UsuarioId, usuario => usuario.Id,
                    (colaborador, usuario) => AdicionarColaboradorProfissional(colaborador, usuario));

            return await Task.FromResult(new PaginacaoDTO<ColaboradorProfissionalDTO>
            {
                paginaAtual = paginacao?.paginaAtual ?? 1,
                totalPaginas = paginacao?.totalPaginas ?? 1,
                quantidadePagina = paginacao?.quantidadePagina ?? 10,
                registroInicial = paginacao?.registroInicial ?? 1,
                objetoPesquisa = paginacao?.objetoPesquisa ?? new ColaboradorProfissionalDTO(),
                total = paginacao?.total ?? 0,
                objetos = colaboradoresFiltrados?.ToList() ?? new List<ColaboradorProfissionalDTO>()
            });
        }

        private static ColaboradorProfissionalDTO AdicionarColaboradorProfissional(ColaboradorProfissionalDTO colaborador, UsuarioDTO usuario)
        {
            return new ColaboradorProfissionalDTO
            {
                Id = colaborador.Id,
                ColaboradorId = colaborador.ColaboradorId,
                UsuarioId = colaborador.UsuarioId,
                NomeColaborador = usuario.Nome,
                LojaId = colaborador.LojaId,
                RazaoSocial = colaborador.RazaoSocial,
                ServicoId = colaborador.ServicoId,
                DescricaoServico = colaborador?.DescricaoServico,
                Descricao = colaborador?.Descricao,
            };
        }

        public async Task<PaginacaoDTO<ColaboradorProfissionalDTO>> Filtrar(PaginacaoDTO<ColaboradorProfissionalDTO> paginacao)
        {
            paginacao.registroInicial = (paginacao.paginaAtual - 1) * paginacao.quantidadePagina;

            var filtrados = paginacao?.objetos?.Where(objeto =>
                 (string.IsNullOrEmpty(paginacao?.objetoPesquisa?.NomeColaborador) ||
                 objeto.NomeColaborador?.Contains(paginacao.objetoPesquisa.NomeColaborador, StringComparison.OrdinalIgnoreCase) == true) &&

                (string.IsNullOrEmpty(paginacao?.objetoPesquisa?.RazaoSocial) ||
                 objeto.RazaoSocial?.Contains(paginacao.objetoPesquisa.RazaoSocial, StringComparison.OrdinalIgnoreCase) == true) &&

                (string.IsNullOrEmpty(paginacao?.objetoPesquisa?.DescricaoServico) ||
                 objeto.DescricaoServico?.Contains(paginacao.objetoPesquisa.DescricaoServico, StringComparison.OrdinalIgnoreCase) == true) &&

                (string.IsNullOrEmpty(paginacao?.objetoPesquisa?.Descricao) ||
                 objeto.Descricao?.Contains(paginacao.objetoPesquisa.Descricao, StringComparison.OrdinalIgnoreCase) == true)

            ) ?? Enumerable.Empty<ColaboradorProfissionalDTO>();
            paginacao!.total = filtrados?.Count() ?? 0;
            paginacao!.totalPaginas = (paginacao.total + paginacao.quantidadePagina - 1) / paginacao.quantidadePagina;

            paginacao.objetos = filtrados?.Skip(paginacao.registroInicial).Take(paginacao.quantidadePagina);


            return await Task.FromResult(paginacao);
        }
    }
}
