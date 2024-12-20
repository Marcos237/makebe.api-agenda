using api.makebe.agenda.domain.DTO;
using AutoMapper;
using lib.makebe.domain.Entidades;

namespace api.makebe.agenda.applications.AutoMapper
{
    public class UsuarioSessaoMapper : Profile
    {
        public UsuarioSessaoMapper()
        {
            CreateMap<UsuarioDTO, UsuarioSessao>()
                .ForMember(dest => dest.Id ,  opt => opt.Ignore())
                .ForMember(dest => dest.UsuarioId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.Nome))
                .ForMember(dest => dest.UrlImagem, opt => opt.MapFrom(src => src.UrlImagem))
                .ForMember(dest => dest.NomeImagem, opt => opt.MapFrom(src => src.NomeImagem))
                .ReverseMap();
        }
    }
}
