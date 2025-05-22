using api.makebe.agenda.domain.Constants;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Specifications.TextoSpecifications;
using FluentValidation;

namespace api.makebe.agenda.domain.Validations
{
    public class AgendaValidation : AbstractValidator<Agenda>
    {
        public AgendaValidation()
        {
            RuleFor(agenda => new DataValidaSpecification().IsSatisfiedBy(agenda.AgendaAbertaInicio!))
                    .Must(agenda => agenda)
                    .WithMessage(AgendaConstant.DataAberturaInvalido)
                    .WithName(nameof(Agenda.AgendaAbertaInicio));

            RuleFor(agenda => new DataValidaSpecification().IsSatisfiedBy(agenda.AgendaAbertaFim!))
                    .Must(agenda => agenda)
                    .WithMessage(AgendaConstant.DataFechamentoInvalido)
                    .WithName(nameof(Agenda.AgendaAbertaInicio));

            RuleFor(agenda => agenda).Custom((agenda, context) =>
            {
                var isValid = new DatasValidasEntreInicioFimSpecification().IsSatisfiedBy((agenda.AgendaAbertaInicio, agenda.AgendaAbertaFim));
                if (!isValid)
                {
                    context.AddFailure(nameof(Agenda.AgendaAbertaFim), AgendaConstant.DataAbrturaFechamentoInvalido);
                }
            });

            RuleFor(agenda => agenda).Custom((agenda, context) =>
            {
                if (!agenda.IsBloqueadoHoje & (agenda.AgendaBloqueadaInicio != null && agenda.AgendaBloqueadaFim != null))
                {
                    var isValid = new DatasValidasEntreInicioFimSpecification().IsSatisfiedBy((agenda.AgendaBloqueadaInicio, agenda.AgendaBloqueadaFim));
                    if (!isValid)
                    {
                        context.AddFailure(nameof(Agenda.AgendaAbertaFim), AgendaConstant.DataBloqueioFechamentoInvalido);
                    }
                }
            });
        }
    }
}
