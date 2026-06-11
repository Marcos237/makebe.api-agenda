using api.makebe.agenda.applications.Models.Payloads;
using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Extensions;
using api.makebe.agenda.domain.Helpers;
using AutoMapper;

namespace api.makebe.agenda.applications.Mappers.AgendamentoMappers
{
    public class AgendamentoDtoToAgendamentoMap : Profile
    {
        public AgendamentoDtoToAgendamentoMap()
        {
            CreateMap<AgendamentoDTO, Agendamento>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
               .ForMember(dest => dest.IdAgendaColaborador, opt => opt.MapFrom(src => src.IdAgendaColaborador))
               .ForMember(dest => dest.IdServico, opt => opt.MapFrom(src => src.IdServico))
               .ForMember(dest => dest.IdUsuario, opt => opt.MapFrom(src => PropiedadesHelper.ParseGuidOrDefault(src.IdUsuario)))
               .ForMember(dest => dest.DataInicioAgendamento, opt => opt.MapFrom(src => ValoresHelper.MontarDate(src.DataInicioAgendamentoExtenso, src.Data)))
               .ForMember(dest => dest.DataTerminoAgendamento, opt => opt.MapFrom(src => src.MontarDataTermino()));
        }
    }
}
