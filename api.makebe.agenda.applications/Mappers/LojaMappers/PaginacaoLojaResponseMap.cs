using api.makebe.agenda.domain.DTO;
using AutoMapper;

namespace api.makebe.agenda.applications.Mappers.LojaMappers
{
    public class PaginacaoLojaResponseMap : Profile
    {
        public PaginacaoLojaResponseMap()
        {
            CreateMap<PaginacaoDTO<LojaEnderecoDTO>, PaginacaoDTO<LojaResponse>>()
             .ReverseMap();
        }
    }
}
