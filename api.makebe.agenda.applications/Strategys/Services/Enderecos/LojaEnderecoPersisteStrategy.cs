using api.makebe.agenda.applications.Models.Payloads;
using api.makebe.agenda.applications.Strategys.Interfaces.Enderecos;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Enums;
using api.makebe.agenda.domain.Interfaces.Services;
using AutoMapper;

namespace api.makebe.agenda.applications.Strategys.Services.Enderecos
{
    public class LojaEnderecoPersisteStrategy : IEnderecoPersisteStrategy<EnderecoPayload>
    {
        private readonly ILojaEnderecoDomainService _lojaEnderecoDomainService;
        private readonly IMapper _mapper;
        public LojaEnderecoPersisteStrategy(ILojaEnderecoDomainService lojaEnderecoDomainService, IMapper mapper)
        {
            _lojaEnderecoDomainService = lojaEnderecoDomainService;
            _mapper = mapper;
        }
        public async Task<int> Salvar(EnderecoPayload item)
        {
            var itemNaoSalvo = 0;
            if (item.TipoUsuarioId == (int)TipoUsuario.Loja)
            {
                var lojaMap = _mapper.Map<LojaEndereco>(item);
                var response = await _lojaEnderecoDomainService.Salvar(lojaMap);
                return response == 0 ? 0 : response;
            }
            return itemNaoSalvo;
        }
    }
}
