using api.makebe.agenda.applications.Models.Payloads;
using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.infra.crosscutting.Entidades;
using AutoMapper;

namespace api.makebe.agenda.applications.Mappers.PaginacaoMappers
{
    public class PaginacaoDTOMap : Profile
    {
        public PaginacaoDTOMap()
        {
            CreatePaginacaoMap<AgendaPayload, AgendaDTO>();
            CreatePaginacaoMap<LojaPayload, LojaDTO>();
            CreatePaginacaoMap<LojaDTO, LojaResponse>();
            PaginacaoEvent();

        }

        private void CreatePaginacaoMap<TSource, TDestination>()
            where TSource : class
            where TDestination : class
        {
            CreateMap<TSource, TDestination>().ReverseMap();

            CreateMap<PaginacaoDTO<TSource>, PaginacaoDTO<TDestination>>()
                .ForMember(dest => dest.quantidadePagina, opt => opt.MapFrom(src => src.quantidadePagina))
                .ForMember(dest => dest.totalPaginas, opt => opt.MapFrom(src => src.totalPaginas))
                .ForMember(dest => dest.total, opt => opt.MapFrom(src => src.total))
                .ForMember(dest => dest.paginaAtual, opt => opt.MapFrom(src => src.paginaAtual))
                .ForMember(dest => dest.registroInicial, opt => opt.MapFrom(src => src.registroInicial))
                .ForMember(dest => dest.objetos, opt => opt.MapFrom(src => src.objetos))
                .ForMember(dest => dest.objetoPesquisa, opt => opt.MapFrom(src => src.objetoPesquisa))
                .ForMember(dest => dest.idsPesquisa, opt => opt.MapFrom(src => src.idsPesquisa))
                .ReverseMap();
        }
        private void PaginacaoEvent()
        {
            CreateMap<PaginacaoDTO<UsuarioDTO>, PaginacaoEvent<UsuarioEvent>>()
              .ForMember(dest => dest.quantidadePagina, opt => opt.MapFrom(src => src.quantidadePagina))
              .ForMember(dest => dest.totalPaginas, opt => opt.MapFrom(src => src.totalPaginas))
              .ForMember(dest => dest.total, opt => opt.MapFrom(src => src.total))
              .ForMember(dest => dest.paginaAtual, opt => opt.MapFrom(src => src.paginaAtual))
              .ForMember(dest => dest.registroInicial, opt => opt.MapFrom(src => src.registroInicial))
              .ForMember(dest => dest.objetos, opt => opt.MapFrom(src => src.objetos))
              .ForMember(dest => dest.objetoPesquisa, opt => opt.MapFrom(src => src.objetoPesquisa))
              .ForMember(dest => dest.idsPesquisa, opt => opt.MapFrom(src => src.idsPesquisa))
              .ReverseMap();
        }

    }
}
