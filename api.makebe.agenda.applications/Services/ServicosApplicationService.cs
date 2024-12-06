using api.makebe.agenda.applications.Interfaces;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Interfaces.Services;

namespace api.makebe.agenda.applications.Services
{
    public class ServicosApplicationService : IServicoApplicationService
    {
        private readonly IServicosDomainService _servicosDomainService;
        public ServicosApplicationService(IServicosDomainService servicosDomainService)
        {
            _servicosDomainService = servicosDomainService;
        }
        public async Task<IEnumerable<Servicos>> BuscarServicos()
        {
           return await _servicosDomainService.BuscarServicos();
        }
    }
}
