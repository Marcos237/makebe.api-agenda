using api.makebe.agenda.domain.Constants;
using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Interfaces.Repositorys;
using api.makebe.agenda.domain.Interfaces.Services;
using api.makebesession.infra.crosscutting.Entidades;
using MassTransit.Initializers;

namespace api.makebe.agenda.domain.Services
{
    public class ColaboradorDomainService : IColaboradorDomainService
    {
        private readonly IColaboradorRepository _colaboradorRepository;
        private readonly IContaColaboradorDomainService _usuarioColaboradorRepository;
        public ColaboradorDomainService(IColaboradorRepository colaboradorRepository, IContaColaboradorDomainService usuarioColaboradorRepository)
        {
            _colaboradorRepository = colaboradorRepository;
            _usuarioColaboradorRepository = usuarioColaboradorRepository;
        }

        public async Task<ColaboradorDTO> BuscarColaboradorPorIdUsuario(Guid id)
        {
            var colaborador = await _colaboradorRepository.BuscarPorUsuarioId(id) ?? new ColaboradorDTO();
            return colaborador;
        }
        public async Task<ColaboradorDTO> BuscarColaboradorPorId(int id)
        {
            return await _colaboradorRepository.BuscarPorId(id);
        }
        public async Task<int> Salvar(Colaborador colaborador)
        {
            colaborador.Status = true;
            colaborador.DataAtualizacao = DateTime.Now;
            if (colaborador.Id == 0)
            {
                colaborador.Datacadastro = DateTime.Now;
                var result = await _colaboradorRepository.Salvar(colaborador);
                return result;
            }
            var resultUpdate = await _colaboradorRepository.Atualizar(colaborador);
            return resultUpdate.Id;
        }

        public async Task<PaginacaoDTO<ColaboradorDTO>> MontarColaboradores(PaginacaoDTO<UsuarioDTO>? paginacao, string usuarioId, IEnumerable<PermissaoEvent> permissoesEvents)
        {
            var colaboradores = await _usuarioColaboradorRepository.BuscarColaboradorPorUsuarioId(usuarioId);

            var colaboradoresFiltrados = paginacao?.objetos
                ?.Where(usuario => colaboradores.Any(colaborador => colaborador.UsuarioId == usuario.Id))
                ?.Join(permissoesEvents,
                       usuario => usuario.PermissaoId.ToString(),
                       permissao => permissao.PermissaoId,
                       (usuario, permissao) =>
                       {
                           var colaborador = colaboradores.First(c => c.UsuarioId == usuario.Id);
                           return AdicionarColaborador(usuario, permissao, colaborador);
                       });

            return new PaginacaoDTO<ColaboradorDTO>
            {
                paginaAtual = paginacao?.paginaAtual ?? 1,
                totalPaginas = paginacao?.totalPaginas ?? 1,
                quantidadePagina = paginacao?.quantidadePagina ?? 10,
                registroInicial = paginacao?.registroInicial ?? 1,
                total = paginacao?.total ?? 0,
                objetos = colaboradoresFiltrados?.ToList() ?? new List<ColaboradorDTO>()
            };
        }
        public async Task<bool> Desativar(int id)
        {
            return await _colaboradorRepository.Desativar(id);
        }

        public async Task<IEnumerable<string>> MontarIdsPesquisas(string usuarioId)
        {
            var usuarioIds = (await _usuarioColaboradorRepository.BuscarColaboradorPorUsuarioId(usuarioId)).Select(colaborador => colaborador.UsuarioId.ToString());
            return usuarioIds;
        }
        private static ColaboradorDTO AdicionarColaborador(UsuarioDTO usuario, PermissaoEvent permissao, ColaboradorDTO colaborador)
        {
            return new ColaboradorDTO
            {
                Id = colaborador.Id,
                UsuarioId = colaborador.UsuarioId,
                Status = usuario.Status,
                DescricaoStatus = usuario.Status ? BaseConstant.Ativo : BaseConstant.Inativo,
                Nome = usuario.Nome,
                Cpf = usuario.Cpf,
                Email = usuario.Email,
                Telefone = usuario.Telefone,
                PermissaoId = usuario.PermissaoId.ToString() ?? string.Empty,
                DescricaoPermissao = permissao.Descricao,
                UrlImagem = usuario.UrlImagem,
                NomeImagem = usuario.NomeImagem
            };
        }
    }
}
