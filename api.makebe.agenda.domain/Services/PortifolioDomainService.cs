using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Interfaces.Repositorys;
using api.makebe.agenda.domain.Interfaces.Services;

namespace api.makebe.agenda.domain.Services
{
    public class PortifolioDomainService : IPortifolioDomainService
    {
        private readonly IPortifolioRepository _PortifolioRepository;

        public PortifolioDomainService(IPortifolioRepository PortifolioRepository)
        {
            _PortifolioRepository = PortifolioRepository;   
        }
        public async Task<PortifolioDTO> BuscarPorId(int id)
        {
            var result = await _PortifolioRepository.BuscarPorId(id);
            return result;
        }
        public async Task<int> Salvar(Portifolio portifolio)
        {
            portifolio.Status = true;
            portifolio.DataAtualizacao = DateTime.Now;
            if (portifolio.Id == 0)
            {
                portifolio.DataCadastro = DateTime.Now;
                var result = await _PortifolioRepository.Salvar(portifolio);
                return result;
            }
            var resultAualizado = await _PortifolioRepository.Atualizar(portifolio);
            return resultAualizado.Id;
        }
        public async Task<Portifolio> Atualizar(Portifolio portifolio)
        {
            var result = await _PortifolioRepository.Atualizar(portifolio);
            return result;
        }
        public async Task<bool> Deastivar(int id)
        {
            var resutl = await _PortifolioRepository.Deastivar(id);
            return resutl;  
        }
    }
}
