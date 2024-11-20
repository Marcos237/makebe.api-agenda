using api.makebe.agenda.domain.Constants;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Specifications.LojaSpecifications;
using FluentValidation;

namespace api.makebe.agenda.domain.Validations
{
    public class LojaEnderecoValidation : AbstractValidator<LojaEndereco>
    {
        public LojaEnderecoValidation()
        {
            RuleFor(lojaId => new LojaNaoPodeSerNuloOuVazioSpecification().IsSatisfiedBy(lojaId.Id))
                    .Must(lojaId => lojaId)
                    .WithMessage(EnderecoConstant.LojaInvalido)
                    .WithName(nameof(LojaEndereco.LojaId));
        }
    }
}
