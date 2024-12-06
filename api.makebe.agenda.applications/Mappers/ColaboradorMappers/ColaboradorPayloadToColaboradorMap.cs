using api.makebe.agenda.applications.Models.Payloads;
using api.makebe.agenda.domain.Entidades;
using AutoMapper;

namespace api.makebe.agenda.applications.Mappers.ColaboradorMappers
{
    public class ColaboradorPayloadToColaboradorMap : Profile
    {
        public ColaboradorPayloadToColaboradorMap()
        {
            CreateMap<ColaboradorPayload, Colaborador>().ReverseMap();
        }
    }
}
