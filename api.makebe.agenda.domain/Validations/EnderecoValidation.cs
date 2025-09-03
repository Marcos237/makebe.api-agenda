using api.makebe.agenda.domain.Constants;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Specifications.EnderecoSpecifications;
using api.makebe.agenda.domain.Specifications.TextoSpecifications;
using FluentValidation;

namespace api.makebe.agenda.domain.Validations
{
    public class EnderecoValidation : AbstractValidator<Endereco>
    {
        public EnderecoValidation()
        {

            RuleFor(endereco => new TextoObrigatorioSpecification().IsSatisfiedBy(endereco.CEP ?? string.Empty))
              .Must(endereco => endereco)
              .WithMessage(EnderecoConstant.CepInvalido)
              .WithName(nameof(Endereco.CEP));

            RuleFor(endereco => new LogradouroSpecifications().IsSatisfiedBy(endereco))
              .Must(endereco => endereco)
              .WithMessage(EnderecoConstant.LogradouroInvalido)
              .WithName(nameof(Endereco.Logradouro));

            RuleFor(endereco => new CepSpecification().IsSatisfiedBy(endereco))
             .Must(endereco => endereco)
             .WithMessage(EnderecoConstant.CepInvalido)
             .WithName(nameof(Endereco.CEP));

            RuleFor(endereco => new CidadeSpecification().IsSatisfiedBy(endereco))
             .Must(endereco => endereco)
             .WithMessage(EnderecoConstant.CidadeInvalida)
             .WithName(nameof(Endereco.Cidade));

            RuleFor(endereco => new EstadoSpecification().IsSatisfiedBy(endereco))
              .Must(endereco => endereco)
              .WithMessage(EnderecoConstant.EstadoInvalido)
              .WithName(nameof(Endereco.Estado));

            RuleFor(endereco => endereco)
                .Must(endereco =>
                {
                    var campos = new List<KeyValuePair<string, int>>
                    {
                       new KeyValuePair<string, int>(endereco.Logradouro ?? string.Empty, 250),
                       new KeyValuePair<string, int>(endereco.Complemento ?? string.Empty, 100),
                       new KeyValuePair<string, int>(endereco.CEP ?? string.Empty, 10),
                       new KeyValuePair<string, int>(endereco.Estado ?? string.Empty, 100),
                       new KeyValuePair<string, int>(endereco.Cidade ?? string.Empty, 250)
                    }
                    .Where(kvp => !string.IsNullOrWhiteSpace(kvp.Key))
                    .DistinctBy(kvp => kvp.Key)
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

                    return new TamanhoCamposSpecification().IsSatisfiedBy(campos);
                })
                .WithMessage(BaseConstant.Campos).WithName("LojaColaborador");
        }
    }
}
