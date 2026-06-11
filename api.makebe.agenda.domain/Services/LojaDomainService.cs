using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Interfaces.Services;
using api.makebe.agenda.infra.data.Repositorys.Interfaces;

namespace api.makebe.agenda.domain.Services
{
    public class LojaDomainService : ILojaDomainService
    {
        private readonly ILojaRepository _lojaRepository;

        public LojaDomainService(ILojaRepository lojaRepository)
        {
            _lojaRepository = lojaRepository;
        }
        public async Task<IEnumerable<LojaDTO>> BuscarTodos(string usuarioId)
        {
            var retorno = await _lojaRepository.BuscarTodos(usuarioId) ?? Enumerable.Empty<LojaDTO>();
            return retorno;
        }

        public async Task<IEnumerable<LojaVitrineDTO>> BuscarLojasVitrinePorTipo(string tipo)
        {
            var retorno = await _lojaRepository.BuscarLojasBannerPorTipo(tipo);
            return retorno;
        }

        public async Task<PaginacaoDTO<LojaDTO>> BuscarTodosPaginado(PaginacaoDTO<LojaDTO> paginacao, string usuarioId)
        {
            var result = await _lojaRepository.BuscarLojas(paginacao, usuarioId);
            result.totalPaginas = (result.total + result.quantidadePagina - 1) / result.quantidadePagina;
            return result;
        }
        public async Task<LojaDTO> BuscarPorId(int id)
        {
            var result = await _lojaRepository.BuscarLojaPorCodigo(id);
            return result;
        }
        public async Task<int> Persitir(Loja loja)
        {
            loja.Status = true;
            loja.DataAtualizacao = DateTime.Now;
            if (loja.Id == 0)
            {
                loja.DataCadastro = DateTime.Now;
                var result = await _lojaRepository.Salvar(loja);
                return result;
            }
            var resultUpdate = await _lojaRepository.Atualizar(loja);
            return resultUpdate.Id;
        }

        public async Task<bool> Desativar(int id)
        {
            var result = await _lojaRepository.Desativar(id);
            return result;
        }
    }
}
