using api.makebe.agenda.applications.Models.Payloads;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Helpers;
using AutoMapper;

namespace api.makebe.agenda.applications.Mappers.EnderecoMappers
{
    public class EnderecoPayloadToEnderecoMap:Profile
    {
        public EnderecoPayloadToEnderecoMap()
        {
            CreateMap<EnderecoPayload, Endereco>()
            .ForMember(dest => dest.CEP, origem => origem.MapFrom(x => TextoHelper.GetNumeros(x.CEP ?? string.Empty)))
                .ReverseMap();
        }
    }
}
