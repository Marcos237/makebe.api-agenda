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

            RuleFor(colaborador => new IdsObrigatoriosSpecifications().IsSatisfiedBy(colaborador.ServicoId!)).Must(colaborador =>
            {
                return colaborador;
            }).WithMessage(ColaboradorProfissionalConstant.ServicoValidacao)
            .WithName(nameof(ColaboradorProfissional.ServicoId));

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

        }
    }
}
