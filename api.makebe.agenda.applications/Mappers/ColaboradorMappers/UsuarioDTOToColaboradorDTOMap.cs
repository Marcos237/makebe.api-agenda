using api.makebe.agenda.domain.DTO;
using AutoMapper;

namespace api.makebe.agenda.applications.Mappers.ColaboradorMappers
{
    public class UsuarioDTOToColaboradorDTOMap : Profile
    {
        public UsuarioDTOToColaboradorDTOMap()
        {
            CreateMap<UsuarioDTO, ColaboradorDTO>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UsuarioId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.Nome))
                .ForMember(dest => dest.Cpf, opt => opt.MapFrom(src => src.Cpf))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Instagram, opt => opt.MapFrom(src => src.Instagran))
                .ForMember(dest => dest.Telefone, opt => opt.MapFrom(src => src.Telefone))
                .ForMember(dest => dest.PermissaoId, opt => opt.MapFrom(src => src.PermissaoId.ToString()))
                .ForMember(dest => dest.NomeImagem, opt => opt.MapFrom(src => src.NomeImagem))
                .ForMember(dest => dest.UrlImagem, opt => opt.MapFrom(src => src.UrlImagem));
        }
    }
}
