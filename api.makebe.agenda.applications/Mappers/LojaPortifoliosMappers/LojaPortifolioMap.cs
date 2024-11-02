using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using AutoMapper;

namespace api.makebe.agenda.applications.Mappers.LojaPortifolios
{
    public class LojaPortifolioMap : Profile
    {
        public LojaPortifolioMap()
        {
            CreateMap<LojaPortifolio, LojaPortifolioDTO>().ReverseMap();    
        }
    }
}
