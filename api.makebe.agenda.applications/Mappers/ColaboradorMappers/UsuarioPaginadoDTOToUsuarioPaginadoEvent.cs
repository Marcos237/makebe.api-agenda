using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.infra.crosscutting.Entidades;
using AutoMapper;

namespace api.makebe.agenda.applications.Mappers.ColaboradorMappers
{
    public class UsuarioPaginadoDTOToUsuarioPaginadoEvent : Profile
    {
        public UsuarioPaginadoDTOToUsuarioPaginadoEvent()
        {
            CreateMap(typeof(PaginacaoEvent<>), typeof(PaginacaoDTO<>))
                .ForMember("quantidadePagina", opt => opt.MapFrom("quantidadePagina"))
                .ForMember("totalPaginas", opt => opt.MapFrom("totalPaginas"))
                .ForMember("total", opt => opt.MapFrom("total"))
                .ForMember("paginaAtual", opt => opt.MapFrom("paginaAtual"))
                .ForMember("registroInicial", opt => opt.MapFrom("registroInicial"))
                .ForMember("objetos", opt => opt.MapFrom("objetos"))
                .ForMember("objetoPesquisa", opt => opt.MapFrom("objetoPesquisa"))
                .ForMember("idsPesquisa", opt => opt.MapFrom("idsPesquisa"))
                .ReverseMap();
        }
    }
}
