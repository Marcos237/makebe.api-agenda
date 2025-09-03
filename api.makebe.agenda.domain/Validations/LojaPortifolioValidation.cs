using api.makebe.agenda.domain.Constants;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Specifications.TextoSpecifications;
using FluentValidation;

namespace api.makebe.agenda.domain.Validations
{
    public class LojaPortifolioValidation : AbstractValidator<LojaPortifolio>
    {
        public LojaPortifolioValidation()
        {
            RuleFor(lojaId => new IdsObrigatoriosSpecifications().IsSatisfiedBy(lojaId!.LojaId))
                    .Must(lojaId => lojaId)
                    .WithMessage(EnderecoConstant.LojaInvalido)
                    .WithName("LojaColaborador");
        }
    }
}

