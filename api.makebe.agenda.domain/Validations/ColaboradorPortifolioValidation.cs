using api.makebe.agenda.domain.Constants;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Specifications.TextoSpecifications;
using FluentValidation;

namespace api.makebe.agenda.domain.Validations
{
    public class ColaboradorPortifolioValidation : AbstractValidator<ColaboradorPortifolio>
    {
        public ColaboradorPortifolioValidation()
        {
            RuleFor(portifolio => new IdsObrigatoriosSpecifications().IsSatisfiedBy(portifolio!.ColaboradorId))
                    .Must(portifolio => portifolio)
                    .WithMessage(ColaboradorProfissionalConstant.ColaboradorIdValidacao)
                    .WithName("LojaColaborador");
        }
    }
}
