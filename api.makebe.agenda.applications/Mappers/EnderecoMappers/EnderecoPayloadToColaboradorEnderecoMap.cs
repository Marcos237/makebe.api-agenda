using api.makebe.agenda.applications.Models.Payloads;
using api.makebe.agenda.domain.Entidades;
using AutoMapper;

namespace api.makebe.agenda.applications.Mappers.EnderecoMappers
{
    public class EnderecoPayloadToColaboradorEnderecoMap : Profile
    {
        public EnderecoPayloadToColaboradorEnderecoMap()
        {
            CreateMap<EnderecoPayload, ColaboradorEndereco>()
                .ForMember(dest => dest.ColaboradorId, opt => opt.MapFrom(src => src.ColaboradorId))
                .ForMember(dest => dest.EnderecoId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ColaboradorEnderecoId))
                .ReverseMap();
        }
    }
}
