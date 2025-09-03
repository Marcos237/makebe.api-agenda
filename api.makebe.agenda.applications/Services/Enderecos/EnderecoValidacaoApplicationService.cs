using api.makebe.agenda.applications.Interfaces;
using api.makebe.agenda.applications.Models.Payloads;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Enums;
using api.makebe.agenda.domain.Interfaces.Services;
using AutoMapper;

namespace api.makebe.agenda.applications.Services.Enderecos
{
    public class EnderecoValidacaoApplicationService : IEnderecoValidacaoApplicationService
    {
        private readonly IValidationService<Endereco> _validationService;
        private readonly IValidationService<LojaEndereco> _validationLojaService;
        private readonly IValidationService<ColaboradorEndereco> _validationColaboradorService;
        private readonly IMapper _mapper;

        public EnderecoValidacaoApplicationService(IValidationService<Endereco> validationService, IValidationService<LojaEndereco> validationLojaService,
            IValidationService<ColaboradorEndereco> validationColaboradorService, IMapper mapper)
        {
            _validationService = validationService;
            _validationColaboradorService = validationColaboradorService;
            _validationLojaService = validationLojaService;
            _mapper = mapper;
        }

        public void RetornarListaVazia(string entidade, string mensagem)
        {
            _validationService.RetornarListaVazia(entidade, mensagem);
        }

        public async Task<bool> Validar(EnderecoPayload enderecoPayload)
        {
            var endereco = _mapper.Map<Endereco>(enderecoPayload);
            var isValidate = await _validationService.Validar(endereco);

            if (enderecoPayload.TipoUsuarioId == (int)TipoUsuario.Loja)
            {
                var lojaMap = _mapper.Map<LojaEndereco>(enderecoPayload);
                isValidate = await _validationLojaService.Validar(lojaMap);
                return isValidate;
            }
            var colaboradorMap = _mapper.Map<ColaboradorEndereco>(enderecoPayload);
            isValidate = await _validationColaboradorService.Validar(colaboradorMap);
            return isValidate;
        }
    }
}
