using api.makebe.agenda.domain.Constants;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Specifications.TextoSpecifications;
using FluentValidation;

namespace api.makebe.agenda.domain.Validations
{
    public class ColaboradorProfissionalValidation : AbstractValidator<ColaboradorProfissional>
    {
        public ColaboradorProfissionalValidation()
        {
            RuleFor(colaborador => new IdsObrigatoriosSpecifications().IsSatisfiedBy(colaborador.ColaboradorId!)).Must(colaborador =>
            {
                return colaborador;
            }).WithMessage(ColaboradorProfissionalConstant.ColaboradorIdValidacao)
            .WithName(nameof(ColaboradorProfissional.ColaboradorId));

            RuleFor(colaborador => new IdsObrigatoriosSpecifications().IsSatisfiedBy(colaborador.LojaId!)).Must(colaborador =>
            {
                return colaborador;
            }).WithMessage(ColaboradorProfissionalConstant.LojaIdValidacao)
            .WithName(nameof(ColaboradorProfissional.LojaId));

            RuleFor(colaborador => colaborador).Must(colaborador =>
            {
                var campos = new List<KeyValuePair<string, int>>
                {
                  new KeyValuePair<string, int>(colaborador!.Descricao ?? string.Empty, 200),
                }
                .Where(kvp => !string.IsNullOrWhiteSpace(kvp.Key))
                .DistinctBy(kvp => kvp.Key)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
                return new TamanhoCamposSpecification().IsSatisfiedBy(campos);
            })
            .WithMessage(BaseConstant.Campos)
            .WithName("Descricao");

            RuleFor(colaborador => colaborador.Servicos)
                .Must(servicos => servicos == null || servicos.Count() <= 10)
                .WithMessage(ColaboradorProfissionalConstant.ServicoQuantidadeValidacao)
                .WithName(nameof(ColaboradorProfissional.Servicos));

            RuleFor(colaborador => colaborador.Servicos)
                .Must(servicos => servicos == null || servicos.GroupBy(x => x.IdServico).All(x => x.Count() == 1))
                .WithMessage(ColaboradorProfissionalConstant.ServicoDuplicadoValidacao)
                .WithName(nameof(ColaboradorProfissional.Servicos));

        }
    }
}
