using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Interfaces.Repositorys;
using api.makebe.agenda.domain.Interfaces.Services;

namespace api.makebe.agenda.domain.Services
{
    public class ColaboradorDomainService : IColaboradorDomainService
    {
        private readonly IColaboradorRepository _colaboradorRepository;
        public ColaboradorDomainService(IColaboradorRepository colaboradorRepository)
        {
            _colaboradorRepository = colaboradorRepository;
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

        public async Task<PaginacaoDTO<ColaboradorDTO>> MontarColaboradores(PaginacaoDTO<UsuarioDTO>? paginacao, string usuarioId)
        {
            var colaboradores = await _colaboradorRepository.BuscarBuscarColaboradoresPorId(usuarioId);
            var colaboradoresFiltrados = colaboradores.Select(colaborador =>
            {
                colaborador.Usuario = paginacao?.objetos?.FirstOrDefault(usuario => usuario.Id == colaborador.UsuarioId);
                return colaborador;
            });

            return new PaginacaoDTO<ColaboradorDTO>
            {
                paginaAtual = paginacao?.paginaAtual ?? 1,
                totalPaginas = paginacao?.totalPaginas ?? 10,
                quantidadePagina = paginacao?.quantidadePagina ?? 10,
                registroInicial = paginacao?.registroInicial ?? 1,
                objetos = colaboradoresFiltrados,
            };
        }

        public async Task<bool> Desativar(int id)
        {
            return await _colaboradorRepository.Desativar(id);
        }
    }
}
