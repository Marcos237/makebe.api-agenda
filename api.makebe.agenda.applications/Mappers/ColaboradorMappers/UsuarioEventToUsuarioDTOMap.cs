using api.makebe.agenda.domain.DTO;
using api.makebesession.infra.crosscutting.Entidades;
using AutoMapper;

namespace api.makebe.agenda.applications.Mappers.ColaboradorMappers
{
    public class UsuarioEventToUsuarioDTOMap : Profile
    {
        public UsuarioEventToUsuarioDTOMap()
        {
            CreateMap<UsuarioDTO, UsuarioEvent>()
                .ForMember(dest => dest.DataCadastro, opt => opt.Ignore()) 
                .ForMember(dest => dest.DataAtualizacao, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}
