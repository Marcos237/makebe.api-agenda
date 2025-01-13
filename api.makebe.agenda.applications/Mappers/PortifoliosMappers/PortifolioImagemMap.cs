using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using AutoMapper;

namespace api.makebe.agenda.applications.Mappers.LojaPortifolios
{
    public class PortifolioImagemMap : Profile
    {
        public PortifolioImagemMap()
        {
            CreateMap<PortifolioImagemDTO, PortifolioImagens>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.LojaPortifolioImagemId)).ReverseMap();
        }
    }
}
