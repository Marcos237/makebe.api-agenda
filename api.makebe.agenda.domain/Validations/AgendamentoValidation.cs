using api.makebe.agenda.domain.Constants;
using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Helpers;
using api.makebe.agenda.domain.Interfaces.Repositorys;
using api.makebe.agenda.domain.Specifications.AgendamentoSpecifications;
using api.makebe.agenda.domain.Specifications.TextoSpecifications;
using FluentValidation;

namespace api.makebe.agenda.domain.Validations
{
    public class AgendamentoValidation : AbstractValidator<AgendamentoDTO>
    {
        private readonly IAgendamentoLojaRepository _agendamentoLojaRepository;
        private readonly IAgendamentoColaboradorRepository _agendamentoColaboradorRepository;
        public AgendamentoValidation(IAgendamentoLojaRepository agendamentoLojaRepository, IAgendamentoColaboradorRepository agendamentoColaboradorRepository)
        {
            _agendamentoLojaRepository = agendamentoLojaRepository; 
            _agendamentoColaboradorRepository = agendamentoColaboradorRepository;

            RuleFor(agendamento => new AgendamentoLojaPeriodoSpecification(_agendamentoLojaRepository).IsSatisfiedBy(agendamento))
                    .Must(agendamento => agendamento)
                    .WithMessage(AgendamentoConstant.AgendamentoLojaFechada)
                    .WithName(nameof(Agendamento.DataInicioAgendamento));

            RuleFor(agendamento => new AgendamentoLojaBloqueadaSpecification(_agendamentoLojaRepository).IsSatisfiedBy(agendamento))
                    .Must(agendamento => agendamento)
                    .WithMessage(AgendamentoConstant.AgendamentoLojaBloqueada)
                    .WithName(nameof(Agendamento.DataInicioAgendamento));

            RuleFor(agendamento => new AgendamentoColaboradorAgendaPeriodoSpecification(_agendamentoColaboradorRepository).IsSatisfiedBy(agendamento))
                    .Must(agendamento => agendamento)
                    .WithMessage(AgendamentoConstant.AgendamentoColaboradorFechado)
                    .WithName(nameof(Agendamento.DataInicioAgendamento));

            RuleFor(agendamento => new AgendamentoColaboradorAgendaBloqueadaSpecification(_agendamentoColaboradorRepository).IsSatisfiedBy(agendamento))
                    .Must(agendamento => agendamento)
                    .WithMessage(AgendamentoConstant.AgendamentoColaboradorFechado)
                    .WithName(nameof(Agendamento.DataInicioAgendamento));

            RuleFor(agendamento => new AgendamentoDisponivelSpecification(_agendamentoColaboradorRepository).IsSatisfiedBy(agendamento))
                    .Must(agendamento => agendamento)
                    .WithMessage(AgendamentoConstant.AgendamentoIndiponivel)
                    .WithName(nameof(Agendamento.DataInicioAgendamento));


            RuleFor(agendamento => new DataValidaSpecification().IsSatisfiedBy(agendamento.DataInicioAgendamento!))
                    .Must(agendamento => agendamento)
                    .WithMessage(AgendaConstant.DataAberturaInvalido)
                    .WithName(nameof(Agendamento.DataInicioAgendamento));

            RuleFor(agendamento => new DataValidaSpecification().IsSatisfiedBy(agendamento.DataTerminoAgendamento!))
                    .Must(agendamento => agendamento)
                    .WithMessage(AgendaConstant.DataFechamentoInvalido)
                    .WithName(nameof(Agendamento.DataTerminoAgendamento));

            RuleFor(agendamento => agendamento).Custom((agendamento, context) =>
            {
                var isValid = new DatasValidasEntreInicioFimSpecification().IsSatisfiedBy(
                    (ValoresHelper.MontarDate(agendamento?.DataInicioAgendamentoExtenso, agendamento?.Data),
                    ValoresHelper.MontarDate(agendamento?.DataTerminoAgendamentoExtenso, agendamento?.Data)));
                if (!isValid)
                {
                    context.AddFailure(nameof(Agendamento.DataInicioAgendamento), AgendaConstant.DataAbrturaFechamentoInvalido);
                }
            });

            RuleFor(agendamento => agendamento).Custom((agendamento, context) =>
            {
                var isValid = new TextoObrigatorioSpecification().IsSatisfiedBy((agendamento?.IdUsuario ?? string.Empty));
                if (!isValid)
                {
                    context.AddFailure("Cliente", AgendamentoConstant.ClienteObrigatorio);
                }
            });

            RuleFor(agendamento => agendamento).Custom((agendamento, context) =>
            {
                var isValid = new IdsObrigatoriosSpecifications().IsSatisfiedBy((agendamento.IdServico));
                if (!isValid)
                {
                    context.AddFailure("Servico", AgendamentoConstant.ServicoObrigatorio);
                }
            });
        }
    }
}
