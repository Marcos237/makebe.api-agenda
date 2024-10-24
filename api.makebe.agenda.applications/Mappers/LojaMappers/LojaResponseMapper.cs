using api.makebe.agenda.domain.Entidades;
using AutoMapper;

namespace api.makebe.agenda.applications.Mappers.LojaMappers
{
    public class LojaResponseMapper : Profile
    {
        public LojaResponseMapper()
        {
            CreateMap<Loja, LojaResponse>()
                .ForMember(dest => dest.CNPJ, origem => origem.MapFrom(item => item.CNPJ!.Codigo ?? string.Empty));
        }
    }
}
