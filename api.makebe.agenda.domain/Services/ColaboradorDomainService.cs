using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Interfaces.Repositorys;
using api.makebe.agenda.domain.Interfaces.Services;

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
        public async Task<int> Salvar(Colaborador colaborador, string id)
        {
            colaborador.DataAtualizacao = DateTime.Now;
            if (string.IsNullOrEmpty(id))
            {
                colaborador.Datacadastro = DateTime.Now;
                var result = await _colaboradorRepository.Salvar(colaborador);
                return result;
            }
            var resultUpdate = await _colaboradorRepository.Atualizar(colaborador);
            return resultUpdate.Id;
        }

       public async Task<IEnumerable<ColaboradorDTO>> BuscarPorConta(string usuarioId)
        {
            var response = await _colaboradorRepository.BuscarPorConta(usuarioId);
            return response;
        }


        public async Task<bool> Desativar(int id)
        {
            return await _colaboradorRepository.Desativar(id);
        }
        public async Task<PaginacaoDTO<ColaboradorDTO>> BuscarPaginadoPorConta(string usuarioId, PaginacaoDTO<ColaboradorDTO> paginacao)
        {
            var colaboradores = await _colaboradorRepository.BuscarPaginadoPorConta(usuarioId, paginacao);
            colaboradores.totalPaginas = (colaboradores.total + colaboradores.quantidadePagina - 1) / colaboradores.quantidadePagina;
            return colaboradores;
        }
    }
}
