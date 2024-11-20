using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Helpers;
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
               .ForMember(dest => dest.TipoArquivo, opt => opt.MapFrom(src => FileHelpers.GetExtensaoArquivo(src.NomeImagem ?? string.Empty)))
               .ForMember(dest => dest.TituloImagem, opt => opt.MapFrom(src => src.TituloImagem))
               .ReverseMap();

            CreateMap<Arquivo, LojaPortifolioImagens>()
                .ForPath(dest => dest.Imagem!.UrlImagem, opt => opt.MapFrom(src => src.UrlImagem ?? string.Empty))
                .ForPath(dest => dest.Imagem!.NomeArquivo, opt => opt.MapFrom(src => src.NomeArquivo ?? string.Empty))
                .ForPath(dest => dest.Imagem!.TipoArquivo, opt => opt.MapFrom(src => src.TipoArquivo ?? string.Empty))
                .ForMember(dest => dest.TituloImagem, opt => opt.MapFrom(src => src.TituloImagem))
                .ReverseMap();

        }
    }
}
