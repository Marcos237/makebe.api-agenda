using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using AutoMapper;

namespace api.makebe.agenda.applications.Mappers.LojaPortifoliosMappers
{
    public class LojaPortifolioImagensArquivoMap : Profile
    {
        public LojaPortifolioImagensArquivoMap()
        {
            CreateMap<LojaPortifolioImagemDTO, Arquivo>()
               .ForMember(dest => dest.UrlImagem, opt => opt.MapFrom(src => src.UrlImagem))
               .ForMember(dest => dest.NomeArquivo, opt => opt.MapFrom(src => src.NomeImagem))          
               .ReverseMap();
        }
    }
}
