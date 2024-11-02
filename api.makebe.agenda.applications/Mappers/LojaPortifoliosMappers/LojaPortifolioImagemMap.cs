using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using AutoMapper;

namespace api.makebe.agenda.applications.Mappers.LojaPortifolios
{
    public class LojaPortifolioImagemMap : Profile
    {
        public LojaPortifolioImagemMap()
        {
            CreateMap<LojaPortifolioImagemDTO, LojaPortifolioImagens>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.LojaPortifolioImagemId)).ReverseMap();
        }
    }
}
