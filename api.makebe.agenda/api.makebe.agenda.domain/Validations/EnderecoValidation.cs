using api.makebe.agenda.domain.Constants;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Specifications.EnderecoSpecifications;
using FluentValidation;

namespace api.makebe.agenda.domain.Validations
{
    public class EnderecoValidation : AbstractValidator<Endereco>
    {
        public EnderecoValidation()
        {
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
        }
    }
}
