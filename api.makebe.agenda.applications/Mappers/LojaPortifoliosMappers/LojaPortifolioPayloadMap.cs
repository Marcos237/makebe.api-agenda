using api.makebe.agenda.applications.Models.Payloads;
using api.makebe.agenda.domain.Entidades;
using AutoMapper;

namespace api.makebe.agenda.applications.Mappers.LojaPortifolios
{
    public class LojaPortifolioPayloadMap : Profile
    {
        public LojaPortifolioPayloadMap()
        {
            CreateMap<LojaPortifolioPayload, LojaPortifolio>().ReverseMap();
        }
    }
}
