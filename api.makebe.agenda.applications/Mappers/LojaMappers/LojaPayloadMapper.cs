using api.makebe.agenda.applications.Models.Payloads;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.ValueObjects;
using AutoMapper;

namespace api.makebe.agenda.applications.Mappers.LojaMappers
{
    public class LojaPayloadMapper : Profile
    {
        public LojaPayloadMapper()
        {
            CreateMap<LojaPayload, Loja>()
                .ForMember(dest => dest.CNPJ, origem => origem.MapFrom(x => x.CNPJ))
                .AfterMap((origem, destino) =>
                {
                    var cnpj = new CNPJ(origem.CNPJ ?? string.Empty);
                    destino.CNPJ = cnpj;
                });
        }
    }
}
