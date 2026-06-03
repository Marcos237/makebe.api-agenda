using api.makebe.agenda.applications.Models.Payloads;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Helpers;
using AutoMapper;

namespace api.makebe.agenda.applications.Mappers.ColaboradorProfissionalMapper
{
    public class ColaboradorProfissionalPayloadToColaboradoProfissionalMap : Profile
    {
        public ColaboradorProfissionalPayloadToColaboradoProfissionalMap()
        {
            CreateMap<ColaboradorProfissionalPayload, ColaboradorProfissional>()
                .ForMember(dest => dest.PeriodoInativoInicio,
                    opt => opt.MapFrom(src => ValoresHelper.ConverterParaData(src.PeriodoInativoInicioExtenso ?? string.Empty)))
                .ForMember(dest => dest.PeriodoInativoFim,
                    opt => opt.MapFrom(src => ValoresHelper.ConverterParaData(src.PeriodoInativoFimExtenso ?? string.Empty)))
                .ReverseMap();
        }
    }
}
