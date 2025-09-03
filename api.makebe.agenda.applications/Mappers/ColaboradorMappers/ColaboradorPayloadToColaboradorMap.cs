using api.makebe.agenda.applications.Models.Payloads;
using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Helpers;
using AutoMapper;

namespace api.makebe.agenda.applications.Mappers.ColaboradorMappers
{
    public class ColaboradorPayloadToColaboradorMap : Profile
    {
        public ColaboradorPayloadToColaboradorMap()
        {
            CreateMap<ColaboradorPayload, Colaborador>()
               .ForMember(dest => dest.UsuarioId, opt => opt.MapFrom(src => PropiedadesHelper.ParseGuidOrDefault(src.UsuarioId)))
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
               .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
               .ReverseMap();

        }
    }
}
