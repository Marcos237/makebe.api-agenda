using api.makebe.agenda.applications.Models.Payloads;
using api.makebe.agenda.domain.Entidades;
using AutoMapper;

namespace api.makebe.agenda.applications.Mappers.EnderecoMappers
{
    public class EnderecoPayloadToLojaEnderecoMap : Profile
    {
        public EnderecoPayloadToLojaEnderecoMap()
        {
            CreateMap<EnderecoPayload, LojaEndereco>()
                .ForMember(dest => dest.LojaId, opt => opt.MapFrom(src => src.LojaId))
                .ForMember(dest => dest.EnderecoId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.LojaEnderecoId))
                .ReverseMap();
        }
    }
}
