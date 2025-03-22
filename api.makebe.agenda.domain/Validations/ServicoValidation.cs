using api.makebe.agenda.domain.Constants;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Specifications.TextoSpecifications;
using FluentValidation;

namespace api.makebe.agenda.domain.Validations
{
    public class ServicoValidation : AbstractValidator<Servicos>
    {
        public ServicoValidation()
        {
            RuleFor(servico => new TextoObrigatorioSpecification().IsSatisfiedBy(servico!.Descricao!)).Must(servico =>
            {
                return servico;
            }).WithMessage(ServicoConstant.DescricaoInvalido);

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
