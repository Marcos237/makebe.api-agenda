using api.makebe.agenda.applications.Models.Payloads;
using api.makebesession.infra.crosscutting.Entidades;
using api.makebesession.infra.crosscutting.Events.Usuarios;
using AutoMapper;

namespace api.makebe.agenda.applications.Mappers.ColaboradorMappers
{
    public class ColaboradorPayloadToUsuarioConsultadoEventMap : Profile
    {
        public ColaboradorPayloadToUsuarioConsultadoEventMap()
        {
            CreateMap<ColaboradorPayload, UsuarioRegistradoEvent>()
            .ForMember(dest => dest.UsuarioConsultado, opt => opt.MapFrom(src => new UsuarioEvent
            {
                Id = src.UsuarioId,
                Nome = src.Nome,
                Cpf = src.Cpf,
                Email = src.Email,
                Telefone = src.Telefone,
                Instagran = src.Instagran,
                PermissaoId = src.PermissaoId,
                NomeImagem = src.NomeImagem,
                UrlImagem = src.UrlImagem,
                DataCadastro = src.DataCadastro,
                DataAtualizacao = src.DataAtualizacao
            }))
            .ForMember(dest => dest.NotificationContext, opt => opt.Ignore())
            .ForMember(dest => dest.dataEvento, opt => opt.Ignore());
        }
    }
}
