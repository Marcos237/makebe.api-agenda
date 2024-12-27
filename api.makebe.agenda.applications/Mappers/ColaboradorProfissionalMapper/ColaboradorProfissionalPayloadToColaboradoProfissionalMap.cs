using api.makebe.agenda.applications.Models.Payloads;
using api.makebe.agenda.domain.Entidades;
using AutoMapper;

namespace api.makebe.agenda.applications.Mappers.ColaboradorProfissionalMapper
{
    public class ColaboradorProfissionalPayloadToColaboradoProfissionalMap : Profile
    {
        public ColaboradorProfissionalPayloadToColaboradoProfissionalMap()
        {
            CreateMap<ColaboradorProfissionalPayload, ColaboradorProfissional>().ReverseMap();
        }
    }
}
