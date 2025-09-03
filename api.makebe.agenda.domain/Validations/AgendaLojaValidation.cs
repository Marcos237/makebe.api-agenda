using api.makebe.agenda.domain.Constants;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Specifications.TextoSpecifications;
using FluentValidation;

namespace api.makebe.agenda.domain.Validations
{
    public class AgendaLojaValidation : AbstractValidator<AgendaLoja>
    {
        public AgendaLojaValidation()
        {

            RuleFor(loja => new IdsObrigatoriosSpecifications().IsSatisfiedBy(loja.IdLoja!))
                    .Must(loja => loja)
                    .WithMessage(AgendaConstant.LojaInvalido)
                    .WithName(nameof(AgendaLoja.IdLoja));

        }
    }
}
