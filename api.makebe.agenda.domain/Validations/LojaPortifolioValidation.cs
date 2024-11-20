using api.makebe.agenda.domain.Constants;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Specifications.LojaSpecifications;
using api.makebe.agenda.domain.Specifications.TextoSpecifications;
using FluentValidation;

namespace api.makebe.agenda.domain.Validations
{
    public class LojaPortifolioValidation : AbstractValidator<LojaPortifolio>
    {
        public LojaPortifolioValidation()
        {
            RuleFor(portifolio => new TextoObrigatorioSpecification().IsSatisfiedBy(portifolio.Titulo!)).Must((portifolio) =>
            {
                return portifolio;
            }).WithMessage(LojaPortifolioConstant.TituloInvalido);

            RuleFor(portifolio => new LojaNaoPodeSerNuloOuVazioSpecification().IsSatisfiedBy(portifolio.LojaId!)).Must((portifolio) =>
            {
                return portifolio;
            }).WithMessage(LojaPortifolioConstant.LojaInvalido);


            RuleFor(portifolio => portifolio)
            .Must(portifolio =>
            {
                var campos = new List<KeyValuePair<string, int>>
                {
                   new KeyValuePair<string, int>(portifolio.Titulo ?? string.Empty, 200),
                   new KeyValuePair<string, int>(portifolio.SubTitulo ?? string.Empty, 200),
                   new KeyValuePair<string, int>(portifolio.Texto ?? string.Empty, 1500),
                }
                .Where(kvp => !string.IsNullOrWhiteSpace(kvp.Key))
                .DistinctBy(kvp => kvp.Key)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

                return new TamanhoCamposSpecification().IsSatisfiedBy(campos);
            })
            .WithMessage(BaseConstant.Campos);
        }
    }
}
