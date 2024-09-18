using api.makebe.agenda.applications.Interfaces;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Interfaces.Services;

namespace api.makebe.agenda.applications.Services
{
    public class EnderecoApplicationService : IEnderecoApplicationService
    {
        private readonly IEnderecoDomainService _enderecoDomainService;
        private readonly IValidationService<Endereco> _validationService;
        public EnderecoApplicationService(IEnderecoDomainService enderecoDomainService, IValidationService<Endereco> validationService)
        {
            _enderecoDomainService = enderecoDomainService;
            _validationService = validationService;
        }
        public async Task<IEnumerable<Endereco>> BuscarPorLojaId(int lojaId)
        {
            var retorno = await _enderecoDomainService.BuscarPorLojaId(lojaId);
            return retorno;
        }

        public async Task<bool> SalvarEnderecos(IEnumerable<Endereco> enderecos)
        {
            var retorno = false;
            foreach (var enredeco in enderecos)
            {
                retorno = await _enderecoDomainService.Salvar(enredeco) > 0;
            }
            return retorno;
        }

        public async Task<bool> ValidarEnderecos(IEnumerable<Endereco> enderecos)
        {
            var isValid = false;
            foreach (var endereco in enderecos)
            {
                isValid = await _validationService.Validar(endereco);
            }
            return isValid;
        }
    }
}
