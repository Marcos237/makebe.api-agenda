using api.makebe.agenda.applications.Interfaces;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Interfaces.Services;

namespace api.makebe.agenda.applications.Services
{
    public class LojaEnderecoApplicationService : ILojaEnderecoApplicationService
    {
        private readonly ILojaEnderecoDomainService _lojaEnderecoDomainService;
        private readonly IValidationService<LojaEndereco> _validationService;

        public LojaEnderecoApplicationService(IValidationService<LojaEndereco> validationService, ILojaEnderecoDomainService lojaEnderecoDomainService)
        {
            _validationService = validationService;
            _lojaEnderecoDomainService = lojaEnderecoDomainService;
        }
        public async Task<bool> SalvarLojaEndereco(LojaEndereco endereco)
        {
            var isValid = await _validationService.Validar(endereco);
            if(!isValid)
                return false;

            return await _lojaEnderecoDomainService.SalvarLojaEndereco(endereco) > 0;
        }
    }
}
