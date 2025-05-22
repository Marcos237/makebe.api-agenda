using api.makebe.agenda.applications.Models.Payloads;
using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Helpers;
using AutoMapper;

namespace api.makebe.agenda.applications.Mappers.AgendaMappers
{
    public class AgendaPayloadToAgendaMap : Profile
    {
        public AgendaPayloadToAgendaMap()
        {
            CreateMap<Agenda, AgendaPayload>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.IsTodoDia, opt => opt.MapFrom(src => src.IsTodoDia))
                .ForMember(dest => dest.IdAgendaSemanaInicio, opt => opt.MapFrom(src => src.IdAgendaSemanaInicio))
                .ForMember(dest => dest.IdAgendaSemanaFim, opt => opt.MapFrom(src => src.IdAgendaSemanaFim))
                .ForMember(dest => dest.AgendaAbertaInicio, opt => opt.MapFrom(src => src.AgendaAbertaInicio))
                .ForMember(dest => dest.AgendaAbertaFim, opt => opt.MapFrom(src => src.AgendaAbertaFim))
                .ForMember(dest => dest.AgendaBloqueadaInicio, opt => opt.MapFrom(src => src.AgendaBloqueadaInicio.HasValue ? src.AgendaBloqueadaInicio.Value.ToLongDateString() : ""))
                .ForMember(dest => dest.AgendaBloqueadaFim, opt => opt.MapFrom(src => src.AgendaBloqueadaFim.HasValue ? src.AgendaBloqueadaFim.Value.ToLongDateString() : ""))
                .ForMember(dest => dest.Bloqueado, opt => opt.MapFrom(src => src.IsBloqueadoHoje));

              CreateMap<AgendaPayload, Agenda>()
                 .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                 .ForMember(dest => dest.IsTodoDia, opt => opt.MapFrom(src => src.IsTodoDia))
                 .ForMember(dest => dest.IdAgendaSemanaInicio, opt => opt.MapFrom(src => src.IdAgendaSemanaInicio))
                 .ForMember(dest => dest.IdAgendaSemanaFim, opt => opt.MapFrom(src => src.IdAgendaSemanaFim))
                 .ForMember(dest => dest.AgendaAbertaInicio, opt => opt.MapFrom(src => ValoresHelper.SetDateTimeCustomer(src.AgendaAbertaInicio)))
                 .ForMember(dest => dest.AgendaAbertaFim, opt => opt.MapFrom(src => ValoresHelper.SetDateTimeCustomer(src.AgendaAbertaFim)))
                 .ForMember(dest => dest.AgendaBloqueadaInicio, opt => opt.MapFrom(src => ValoresHelper.SetDateTimeCustomer(src.AgendaBloqueadaInicio)))
                 .ForMember(dest => dest.AgendaBloqueadaFim, opt => opt.MapFrom(src => ValoresHelper.SetDateTimeCustomer(src.AgendaBloqueadaFim)))
                 .ForMember(dest => dest.IsBloqueadoHoje, opt => opt.MapFrom(src => src.Bloqueado));

        }
    }
}
