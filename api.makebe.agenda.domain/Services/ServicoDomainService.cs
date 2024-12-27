using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Interfaces.Repositorys;
using api.makebe.agenda.domain.Interfaces.Services;

namespace api.makebe.agenda.domain.Services
{
    public class ServicoDomainService : IServicosDomainService
    {
        private readonly IServicosRepository _servicosRepository;
        public ServicoDomainService(IServicosRepository servicosRepository)
        {
            _servicosRepository = servicosRepository;   
        }
        public async Task<IEnumerable<Servicos>> BuscarServicos(string contaId)
        {
            return await _servicosRepository.BuscarServicos(contaId);
        }
    }
}
