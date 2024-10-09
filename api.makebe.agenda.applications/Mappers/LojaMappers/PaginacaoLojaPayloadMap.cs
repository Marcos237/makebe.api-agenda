using api.makebe.agenda.applications.Models.Payloads;
using api.makebe.agenda.domain.DTO;
using AutoMapper;

namespace api.makebe.agenda.applications.Mappers.LojaMappers
{
    public class PaginacaoLojaPayloadMap : Profile
    {
        public PaginacaoLojaPayloadMap()
        {
            CreateMap<PaginacaoDTO<LojaEnderecoDTO>, PaginacaoDTO<LojaPayload>>()
                .ForMember(dest => dest.objetoPesquisa, opt => opt.MapFrom(src => new LojaPayload
                {
                    Id = src.objetoPesquisa!.Id,
                    RazaoSocial = src.objetoPesquisa.RazaoSocial,
                    CNPJ = src.objetoPesquisa.CNPJ,
                    Email = src.objetoPesquisa.Email,
                    Telefone = src.objetoPesquisa.Telefone,
                    TipoLojaId = src.objetoPesquisa.TipoLojaId
                }))
                .ReverseMap()
                .ForMember(dest => dest.objetoPesquisa, opt => opt.MapFrom(src => new LojaEnderecoDTO
                {
                    Id = src.objetoPesquisa!.Id,
                    RazaoSocial = src.objetoPesquisa.RazaoSocial,
                    CNPJ = src.objetoPesquisa.CNPJ,
                    Email = src.objetoPesquisa.Email,
                    Telefone = src.objetoPesquisa.Telefone,
                    TipoLojaId = src.objetoPesquisa.TipoLojaId
                }));
        }
    }

}
