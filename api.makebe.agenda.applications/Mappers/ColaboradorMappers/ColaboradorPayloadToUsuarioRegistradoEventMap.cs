using api.makebe.agenda.applications.Models.Payloads;
using api.makebe.agenda.domain.Helpers;
using api.makebesession.infra.crosscutting.Entidades;
using api.makebesession.infra.crosscutting.Events.Usuarios;
using AutoMapper;

namespace api.makebe.agenda.applications.Mappers.ColaboradorMappers
{
    public class ColaboradorPayloadToUsuarioRegistradoEventMap : Profile
    {
        public ColaboradorPayloadToUsuarioRegistradoEventMap()
        {
            CreateMap<ColaboradorPayload, UsuarioRegistradoEvent>()
                .ForMember(dest => dest.UsuarioConsultado, opt => opt.MapFrom(src => new UsuarioEvent
                {
                    Id = PropiedadesHelper.ParseGuidOrDefault(src.UsuarioId),
                    Nome = src.Nome,
                    Cpf = src.Cpf,
                    Email = src.Email,
                    Telefone = src.Telefone,
                    Status = src.Status,
                    Instagran = src.Instagram,
                    PermissaoId = PropiedadesHelper.ParseGuidOrDefault(src.PermissaoId),
                    NomeImagem = src.NomeImagem,
                    UrlImagem = src.UrlImagem,
                }))
                .ReverseMap();
        }

    }
}
