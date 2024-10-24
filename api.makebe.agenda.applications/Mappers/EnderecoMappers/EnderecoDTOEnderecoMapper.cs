using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Helpers;
using AutoMapper;

namespace api.makebe.agenda.applications.Mappers.EnderecoMappers
{
    public class EnderecoDTOEnderecoMapper:Profile
    {
        public EnderecoDTOEnderecoMapper()
        {
            CreateMap<EnderecoDTO, Endereco>()
            .ForMember(dest => dest.CEP, origem => origem.MapFrom(x => TextoHelper.GetNumeros(x.CEP ?? string.Empty)))
                .ReverseMap();
        }
    }
}
