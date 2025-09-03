using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Helpers;
using AutoMapper;

namespace api.makebe.agenda.applications.Mappers.ServicoMappers
{
    public class ServicoServicoDTOMap : Profile
    {
        public ServicoServicoDTOMap()
        {
            CreateMap<Servico, ServicoDTO>()
                .ForMember(dest => dest.ValorExtenso, origem => origem.MapFrom(x => ValoresHelper.SetValorExtenso(x.Valor)))
                .ForMember(dest => dest.PeriodoExtenso, origem => origem.MapFrom(x => ValoresHelper.SetPeridoExtenso(x.Periodo)))
                .ReverseMap();
        }
    }
}
