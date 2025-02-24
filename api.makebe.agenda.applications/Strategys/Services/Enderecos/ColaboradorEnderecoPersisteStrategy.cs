using api.makebe.agenda.applications.Models.Payloads;
using api.makebe.agenda.applications.Strategys.Interfaces.Enderecos;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Enums;
using api.makebe.agenda.domain.Interfaces.Services;
using AutoMapper;

namespace api.makebe.agenda.applications.Strategys.Services.Enderecos
{
    public class ColaboradorEnderecoPersisteStrategy : IEnderecoPersisteStrategy<EnderecoPayload>
    {
        private readonly IColaboradorEnderecoDomainService _colaboradorEnderecoDomainService;
        private readonly IMapper _mapper;
        public ColaboradorEnderecoPersisteStrategy(IColaboradorEnderecoDomainService colaboradorEnderecoDomainService,  IMapper mapper)
        {
            _colaboradorEnderecoDomainService = colaboradorEnderecoDomainService;
            _mapper = mapper;
        }
        public async Task<int> Salvar(EnderecoPayload item)
        {
            var itemNaoSalvo = 0;
            if (item.TipoUsuarioId == (int)TipoUsuario.Colaborador)
            {
                var colaboradorMap = _mapper.Map<ColaboradorEndereco>(item);
                var response = await _colaboradorEnderecoDomainService.Salvar(colaboradorMap);
                return response == 0 ? 0 : response;
            }
            return itemNaoSalvo;
        }
    }
}
