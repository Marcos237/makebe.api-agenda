using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using AutoMapper;

namespace api.makebe.agenda.applications.Mappers.EnderecoMappers
{
    public class EnderecoToEnderecoDTOMap : Profile
    {
        public EnderecoToEnderecoDTOMap()
        {
            CreateMap<EnderecoDTO, Endereco>().ReverseMap();
        }
    }
}
