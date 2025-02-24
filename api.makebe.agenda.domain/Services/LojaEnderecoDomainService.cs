using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Interfaces.Services;
using api.makebe.agenda.infra.data.Repositorys;

namespace api.makebe.agenda.domain.Services
{
    public class LojaEnderecoDomainService : ILojaEnderecoDomainService
    {
        private readonly IEnderecoContextRepository<LojaEndereco, EnderecoDTO> _lojaEnderecoRepository;
        public LojaEnderecoDomainService(IEnderecoContextRepository<LojaEndereco, EnderecoDTO> lojaEnderecoRepository)
        {
            _lojaEnderecoRepository = lojaEnderecoRepository;
        }

        public async Task<PaginacaoDTO<EnderecoDTO>> BuscarEnderecos(PaginacaoDTO<EnderecoDTO> paginacao, string contaId)
        {
            var response = await _lojaEnderecoRepository.BuscarEnderecos(contaId);
            paginacao.objetos = response;
            var filtrados = await Filtrar(paginacao);
            return filtrados;
        }

        public async Task<PaginacaoDTO<EnderecoDTO>> Filtrar(PaginacaoDTO<EnderecoDTO> paginacao)
        {
            paginacao.registroInicial = (paginacao.paginaAtual - 1) * paginacao.quantidadePagina;

            var filtrados = paginacao?.objetos?.Where(objeto =>
                 (string.IsNullOrEmpty(paginacao?.objetoPesquisa?.RazaoSocial) ||
                 objeto.RazaoSocial?.Contains(paginacao.objetoPesquisa.RazaoSocial, StringComparison.OrdinalIgnoreCase) == true) &&

                (string.IsNullOrEmpty(paginacao?.objetoPesquisa?.Logradouro) ||
                 objeto.Logradouro?.Contains(paginacao.objetoPesquisa.Logradouro, StringComparison.OrdinalIgnoreCase) == true)) 
                ?? Enumerable.Empty<EnderecoDTO>();

            paginacao!.total = filtrados?.Count() ?? 0;
            paginacao!.totalPaginas = (paginacao.total + paginacao.quantidadePagina - 1) / paginacao.quantidadePagina;

            paginacao.objetos = filtrados?.Skip(paginacao.registroInicial).Take(paginacao.quantidadePagina);
            return await Task.FromResult(paginacao);
        }

        public async Task<int> Salvar(LojaEndereco endereco)
        {
            endereco.DataCadastro = DateTime.Now;
            endereco.Status = true;

            if (endereco.Id == 0)
                return await _lojaEnderecoRepository.Salvar(endereco);

            await _lojaEnderecoRepository.Atualizar(endereco);
            return endereco.Id;
        }
    }
}
