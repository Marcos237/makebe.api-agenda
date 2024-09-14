using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Interfaces.Services;
using api.makebe.agenda.infra.data.Repositorys.Interfaces;

namespace api.makebe.agenda.domain.Services
{
    public class LojaDomainService : IDomainService<Loja>
    {
        private readonly ILojaRepository _lojaRepository;

        public LojaDomainService(ILojaRepository lojaRepository)
        {
            _lojaRepository = lojaRepository;
        }
        public async Task<IEnumerable<Loja>> BuscarTodos(PaginacaoDTO<Loja> paginacao, string usuarioId)
        {
            var result = await _lojaRepository.BuscarLojas(paginacao, usuarioId);
            return result;
        }
        public async Task<Loja> BuscarPorId(int id)
        {
            var result = await _lojaRepository.BuscarLojaPorCodigo(id);
            return result;
        }
        public async Task<int> Salvar(Loja loja)
        {
            var result = await _lojaRepository.Salvar(loja);
            return result;
        }

        public async Task<Loja> Atualizar(Loja loja)
        {
            var result = await _lojaRepository.Atualizar(loja);
            return result;
        }

        public async Task<bool> Desativar(int id)
        {
            var loja = await _lojaRepository.BuscarLojaPorCodigo(id);
            loja.Status = false;
            var result = await _lojaRepository.Atualizar(loja) != null;
            return result;
        }

    }
}
