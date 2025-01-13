using api.makebe.agenda.applications.Models.Payloads;
using api.makebe.agenda.domain.Entidades;
using AutoMapper;

namespace api.makebe.agenda.applications.Mappers.PortifoliosMappers
{
    public class PortifolioPayloadToColaboradorPortifolioMap : Profile
    {
        public PortifolioPayloadToColaboradorPortifolioMap()
        {
            CreateMap<PortifolioPayload, ColaboradorPortifolio>()
                .ForMember(dest => dest.ColaboradorId, opt => opt.MapFrom(src => src.ColaboradorId))
                .ForMember(dest => dest.PortifolioId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ColaboradorPortifolioId))
                .ReverseMap();


        }
    }
}
