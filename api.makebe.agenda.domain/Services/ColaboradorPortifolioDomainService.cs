using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Interfaces.Repositorys;
using api.makebe.agenda.domain.Interfaces.Services;

namespace api.makebe.agenda.domain.Services
{
    public class ColaboradorPortifolioDomainService : IColaboradorPortifolioDomainService
    {
        private readonly IColaboradorPortifolioRepository _colaboradorPortifolioRepository;

        public ColaboradorPortifolioDomainService(IColaboradorPortifolioRepository colaboradorPortifolioRepository)
        {
            _colaboradorPortifolioRepository = colaboradorPortifolioRepository;
        }

        public async Task<PaginacaoDTO<PortifolioDTO>> BuscarPortifolios(PaginacaoDTO<PortifolioDTO> paginacao, string contaId)
        {
            var result =  await _colaboradorPortifolioRepository.BuscarPortifolios(paginacao, contaId);
            return result;
        }

        public async Task<int> Salvar(ColaboradorPortifolio item)
        {
            item.Status = true;
            if (item.Id == 0)
            {
                item.DataCadastro = DateTime.Now;
                var response = await _colaboradorPortifolioRepository.Salvar(item);
                return response;
            }

            await _colaboradorPortifolioRepository.Atualizar(item);
            return item.Id;
        }
    }
}
