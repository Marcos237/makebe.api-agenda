using api.makebe.agenda.applications.Models.Payloads;
using api.makebe.agenda.domain.Entidades;
using AutoMapper;

namespace api.makebe.agenda.applications.Mappers.AgendaMappers
{
    public class AgendaPayloadToAgendaLojaMap : Profile
    {
        public AgendaPayloadToAgendaLojaMap()
        {
            CreateMap<AgendaPayload, AgendaLoja>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.IdLoja, opt => opt.MapFrom(src => src.IdLoja))
                .ForMember(dest => dest.Bloqueado, opt => opt.MapFrom(src => src.IsBloqueadoHoje));


            CreateMap<AgendaLoja, AgendaPayload>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.IdLoja, opt => opt.MapFrom(src => src.IdLoja))
                .ForMember(dest => dest.IsBloqueadoHoje, opt => opt.MapFrom(src => src.Bloqueado));

        }
    }
}
