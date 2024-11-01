using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Interfaces.Repositorys;
using api.makebe.agenda.domain.Interfaces.Services;

namespace api.makebe.agenda.domain.Services
{
    public class LojaPortifolioDomainService : ILojaPortifolioDomainService
    {
        private readonly ILojaPortifolioRepository _lojaPortifolioRepository;
        public LojaPortifolioDomainService(ILojaPortifolioRepository lojaPortifolioRepository)
        {
            _lojaPortifolioRepository = lojaPortifolioRepository;   
        }
        public async Task<PaginacaoDTO<LojaPortifolioDTO>> BuscarLojaPortifolios(PaginacaoDTO<LojaPortifolioDTO> paginacao, string usuarioId)
        {
            var portifolios = await _lojaPortifolioRepository.BuscarLojaPortifolios(paginacao, usuarioId);

            portifolios.totalPaginas = (portifolios.total + portifolios.quantidadePagina - 1) / portifolios.quantidadePagina;
            return portifolios;
        }
        public async Task<LojaPortifolioDTO> BuscarPorId(int id)
        {
            var result = await _lojaPortifolioRepository.BuscarPorId(id);
            return result;
        }
        public async Task<int> Salvar(LojaPortifolio portifolio)
        {
            portifolio.Status = true;
            portifolio.DataAtualizacao = DateTime.Now;
            if (portifolio.Id == 0)
            {
                portifolio.DataCadastro = DateTime.Now;
                var result = await _lojaPortifolioRepository.Salvar(portifolio);
                return result;
            }
            var resultAualizado = await _lojaPortifolioRepository.Atualizar(portifolio);
            return resultAualizado.Id;
        }
        public async Task<LojaPortifolio> Atualizar(LojaPortifolio portifolio)
        {
            var result = await _lojaPortifolioRepository.Atualizar(portifolio);
            return result;
        }
        public async Task<bool> Deastivar(int id)
        {
            var resutl = await _lojaPortifolioRepository.Deastivar(id);
            return resutl;  
        }
    }
}
