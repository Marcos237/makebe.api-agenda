using api.makebe.agenda.applications.Models.Payloads;
using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Helpers;
using AutoMapper;

namespace api.makebe.agenda.applications.Mappers.LojaMappers
{
    public class PaginacaoLojaPayloadMap : Profile
    {
        public PaginacaoLojaPayloadMap()
        {
            CreateMap<PaginacaoDTO<LojaDTO>, PaginacaoDTO<LojaPayload>>()
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
                .ForMember(dest => dest.objetoPesquisa, opt => opt.MapFrom(src => new LojaDTO
                {
                    Id = src.objetoPesquisa!.Id,
                    RazaoSocial = src.objetoPesquisa.RazaoSocial,
                    CNPJ = TextoHelper.GetNumeros(src.objetoPesquisa.CNPJ ?? string.Empty),
                    Email = src.objetoPesquisa.Email,
                    Telefone = src.objetoPesquisa.Telefone,
                    TipoLojaId = src.objetoPesquisa.TipoLojaId
                }));
        }
    }

}
