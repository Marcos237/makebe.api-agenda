using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Helpers;
using api.makebe.agenda.infra.crosscutting.Entidades;
using AutoMapper;

namespace api.makebe.agenda.applications.Mappers.ColaboradorMappers
{
    public class UsuarioEventToUsuarioDTOMap : Profile
    {
        public UsuarioEventToUsuarioDTOMap()
        {
            CreateMap<UsuarioDTO, UsuarioEvent>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => PropiedadesHelper.ParseGuidOrDefault(src.Id)))
                .ForMember(dest => dest.PermissaoId, opt => opt.MapFrom(src => PropiedadesHelper.ParseGuidOrDefault(src.PermissaoId)))
                .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.Nome))
                .ForMember(dest => dest.Cpf, opt => opt.MapFrom(src => TextoHelper.GetNumeros(src.Cpf ?? string.Empty)))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Telefone, opt => opt.MapFrom(src => src.Telefone))
                .ForMember(dest => dest.Instagran, opt => opt.MapFrom(src => src.Instagran))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.NomeImagem, opt => opt.MapFrom(src => src.NomeImagem))
                .ForMember(dest => dest.UrlImagem, opt => opt.MapFrom(src => src.UrlImagem))
                .ForMember(dest => dest.DataCadastro, opt => opt.MapFrom(src => src.DataCadastro))
                .ForMember(dest => dest.DataAtualizacao, opt => opt.MapFrom(src => src.DataAtualizacao))
                .ReverseMap()
                .ForMember(dest => dest.Tipo, opt => opt.Ignore());

        }
    }
}
