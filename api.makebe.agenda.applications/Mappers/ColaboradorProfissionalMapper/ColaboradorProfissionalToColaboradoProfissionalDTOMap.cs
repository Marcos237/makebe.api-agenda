using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using AutoMapper;

namespace api.makebe.agenda.applications.Mappers.ColaboradorProfissionalMapper
{
    public class ColaboradorProfissionalToColaboradoProfissionalDTOMap : Profile
    {
        public ColaboradorProfissionalToColaboradoProfissionalDTOMap()
        {
            CreateMap<ColaboradorProfissional, ColaboradorProfissionalDTO>().ReverseMap();
        }
    }
}
