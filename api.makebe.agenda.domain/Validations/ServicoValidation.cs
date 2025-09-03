using api.makebe.agenda.domain.Constants;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Specifications.TextoSpecifications;
using FluentValidation;

namespace api.makebe.agenda.domain.Validations
{
    public class ServicoValidation : AbstractValidator<Servico>
    {
        public ServicoValidation()
        {
            RuleFor(servico => new TextoObrigatorioSpecification().IsSatisfiedBy(servico!.Descricao!)).Must(servico =>
            {
                return servico;
            }).WithMessage(ServicoConstant.DescricaoInvalido)
            .WithName(nameof(Servico.Descricao));

            RuleFor(servico => new ValorMinimoSpecification().IsSatisfiedBy(servico.Valor)).Must(servico =>
            {
                return servico;
            }).WithMessage(ServicoConstant.CampoValorMaiorQueZero)
             .WithName(nameof(Servico.Valor));


            RuleFor(servico => new PeriodoMinimoSpecification().IsSatisfiedBy(servico.Periodo)).Must(servico =>
            {
                return servico;
            }).WithMessage(ServicoConstant.PeriodoMariorCincoMinutos)
             .WithName(nameof(Servico.Periodo));

            RuleFor(servico => servico)
        .Must(servico =>
        {
            var campos = new List<KeyValuePair<string, int>>
            {
              new KeyValuePair<string, int>(servico!.Descricao ?? string.Empty, 250),
            }
            .Where(kvp => !string.IsNullOrWhiteSpace(kvp.Key))
            .DistinctBy(kvp => kvp.Key)
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            return new TamanhoCamposSpecification().IsSatisfiedBy(campos);
        })
        .WithMessage(BaseConstant.Campos);

        }
    }
}
