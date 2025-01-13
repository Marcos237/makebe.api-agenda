using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Interfaces.Repositorys;

namespace api.makebe.agenda.domain.Interfaces.Services
{
    public class PortifolioImagemDomainService : IPortifolioImagemDomainService
    {
        private readonly IPortifolioImagemRepository _PortifolioImagemRepository;
        public PortifolioImagemDomainService(IPortifolioImagemRepository PortifolioImagemRepository)
        {
            _PortifolioImagemRepository = PortifolioImagemRepository;   
        }
        public async Task<IEnumerable<PortifolioImagemDTO>> BuscarImagensPorIdPortifolio(int id)
        {
            var result = await _PortifolioImagemRepository.BuscarImagensPorIdPortifolio(id);
            return result;
        }
        public async Task<PortifolioImagemDTO> BuscarImagensPorId(int id)
        {
            var result = await _PortifolioImagemRepository.BuscarImagensPorId(id);
            return result;
        }
        public async Task<int> Salvar(PortifolioImagens PortifolioImagens)
        {
            PortifolioImagens.Status = true;
            PortifolioImagens.DataAtualizacao = DateTime.Now;
            if (PortifolioImagens.Id == 0)
            {
                PortifolioImagens.DataCadastro = DateTime.Now;
                var result = await _PortifolioImagemRepository.Salvar(PortifolioImagens);
                return result;
            }
            var resultAualizado = await _PortifolioImagemRepository.Atualizar(PortifolioImagens);
            return resultAualizado.Id;
        }

        public async Task<bool> Desativar(int id)
        {
            var result = await _PortifolioImagemRepository.Desativar(id);
            return result;  
        }
    }
}
