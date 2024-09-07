using api.makebe.agenda.applications.Models.Responses;
using api.makebe.agenda.domain.Entidades;
using AutoMapper;

namespace api.makebe.agenda.applications.Mappers.EnderecoMappers
{
    public class EnderecoResponseMapper:Profile
    {
        public EnderecoResponseMapper()
        {
            CreateMap<EnderecoRespose, Endereco>()
                .ReverseMap();
        }
    }
}
