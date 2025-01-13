using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using AutoMapper;

namespace api.makebe.agenda.applications.Mappers.LojaPortifolios
{
    public class PortifolioMap : Profile
    {
        public PortifolioMap()
        {
            CreateMap<Portifolio, PortifolioDTO>().ReverseMap();    
        }
    }
}
