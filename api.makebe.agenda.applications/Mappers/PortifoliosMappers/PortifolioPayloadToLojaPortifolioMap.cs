using api.makebe.agenda.applications.Models.Payloads;
using api.makebe.agenda.domain.Entidades;
using AutoMapper;

namespace api.makebe.agenda.applications.Mappers.PortifoliosMappers
{
    public class PortifolioPayloadToLojaPortifolioMap : Profile
    {
        public PortifolioPayloadToLojaPortifolioMap()
        {
            CreateMap<PortifolioPayload, LojaPortifolio>()
                .ForMember(dest => dest.LojaId, opt => opt.MapFrom(src => src.LojaId))
                .ForMember(dest => dest.PortifolioId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.LojaPortifolioId))
                .ReverseMap();
        }
    }
}
