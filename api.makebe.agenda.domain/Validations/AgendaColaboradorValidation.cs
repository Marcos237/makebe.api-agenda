using api.makebe.agenda.domain.Constants;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Specifications.TextoSpecifications;
using FluentValidation;

namespace api.makebe.agenda.domain.Validations
{
    public class AgendaColaboradorValidation : AbstractValidator<AgendaColaborador>
    {
        public AgendaColaboradorValidation()
        {
            RuleFor(colaborador => new IdsObrigatoriosSpecifications().IsSatisfiedBy(colaborador.IdColaborador!))
                    .Must(colaborador => colaborador)
                    .WithMessage(AgendaConstant.ColaboradorInvalido)
                    .WithName(nameof(AgendaColaborador.IdColaborador));
        }
    }
}
