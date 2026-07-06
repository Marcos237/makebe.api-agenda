using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Helpers;
using AutoMapper;

namespace api.makebe.agenda.applications.Mappers.ColaboradorMappers
{
    public class ColaboradorProfissionalDTOToColaboradorDTOMap : Profile
    {
        public ColaboradorProfissionalDTOToColaboradorDTOMap()
        {
            CreateMap<ColaboradorProfissionalDTO, ColaboradorDTO>()
                .ForMember(dest => dest.UsuarioId, opt => opt.MapFrom(src =>
                    string.IsNullOrWhiteSpace(src.UsuarioId) ? (Guid?)null : PropiedadesHelper.ParseGuidOrDefault(src.UsuarioId)))
                .ForMember(dest => dest.UsuarioCodigo, opt => opt.MapFrom(src => src.UsuarioId))
                .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.NomeColaborador))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.DescricaoStatus, opt => opt.Ignore())
                .ForMember(dest => dest.Cpf, opt => opt.Ignore())
                .ForMember(dest => dest.Email, opt => opt.Ignore())
                .ForMember(dest => dest.Instagram, opt => opt.Ignore())
                .ForMember(dest => dest.Telefone, opt => opt.Ignore())
                .ForMember(dest => dest.PermissaoId, opt => opt.Ignore())
                .ForMember(dest => dest.DescricaoPermissao, opt => opt.Ignore())
                .ForMember(dest => dest.NomeImagem, opt => opt.Ignore());
        }
    }
}
