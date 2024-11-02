using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Interfaces.Repositorys;

namespace api.makebe.agenda.domain.Interfaces.Services
{
    public class LojaPortifolioImagemDomainService : ILojaPortifolioImagemDomainService
    {
        private readonly ILojaPortifolioImagemRepository _lojaPortifolioImagemRepository;
        public LojaPortifolioImagemDomainService(ILojaPortifolioImagemRepository lojaPortifolioImagemRepository)
        {
            _lojaPortifolioImagemRepository = lojaPortifolioImagemRepository;   
        }
        public async Task<IEnumerable<LojaPortifolioImagemDTO>> BuscarImagensPorIdLojaPortifolio(int id)
        {
            var result = await _lojaPortifolioImagemRepository.BuscarImagensPorIdLojaPortifolio(id);
            return result;
        }
        public async Task<LojaPortifolioImagemDTO> BuscarImagensPorId(int id)
        {
            var result = await _lojaPortifolioImagemRepository.BuscarImagensPorId(id);
            return result;
        }
        public async Task<int> Salvar(LojaPortifolioImagens lojaPortifolioImagens)
        {
            lojaPortifolioImagens.Status = true;
            lojaPortifolioImagens.DataAtualizacao = DateTime.Now;
            if (lojaPortifolioImagens.Id == 0)
            {
                lojaPortifolioImagens.DataCadastro = DateTime.Now;
                var result = await _lojaPortifolioImagemRepository.Salvar(lojaPortifolioImagens);
                return result;
            }
            var resultAualizado = await _lojaPortifolioImagemRepository.Atualizar(lojaPortifolioImagens);
            return resultAualizado.Id;
        }

        public async Task<bool> Desativar(int id)
        {
            var result = await _lojaPortifolioImagemRepository.Desativar(id);
            return result;  
        }
    }
}
