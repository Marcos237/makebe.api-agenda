using api.makebe.agenda.applications.Models.Payloads;
using api.makebe.agenda.domain.Entidades;
using AutoMapper;

namespace api.makebe.agenda.applications.Mappers.ColaboradorMappers
{
    public  class ColaboradorPayloadToLojaColaboradorMap : Profile
    {
        public ColaboradorPayloadToLojaColaboradorMap()
        {
            CreateMap<ColaboradorPayload, LojaColaborador>()
                .ForMember(dest => dest.ColaboradorId , opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.LojaId , opt => opt.MapFrom(src => src.LojaId)).ReverseMap();
        }
    }
}
