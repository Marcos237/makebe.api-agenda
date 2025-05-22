using api.makebe.agenda.applications.Models.Payloads;
using api.makebe.agenda.domain.Entidades;
using AutoMapper;

namespace api.makebe.agenda.applications.Mappers.AgendaMappers
{
    public  class AgendaPayloadToAgendaColaboradorMap : Profile
    {
        public AgendaPayloadToAgendaColaboradorMap()
        {
            CreateMap<AgendaPayload, AgendaColaborador>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.IdColaborador, opt => opt.MapFrom(src => src.IdColaborador))
                .ForMember(dest => dest.Bloqueado, opt => opt.MapFrom(src => src.IsBloqueadoHoje));


            CreateMap<AgendaColaborador, AgendaPayload>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.IdColaborador, opt => opt.MapFrom(src => src.IdColaborador))
                .ForMember(dest => dest.IsBloqueadoHoje, opt => opt.MapFrom(src => src.Bloqueado));
        }
    }
}
