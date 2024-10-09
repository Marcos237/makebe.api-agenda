using api.makebe.agenda.domain.Constants;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Specifications.CnpjSpecifications;
using api.makebe.agenda.domain.Specifications.LojaSpecifications;
using api.makebe.agenda.domain.Specifications.TextoSpecifications;
using FluentValidation;

namespace api.makebe.agenda.domain.Validations
{
    public class LojaValidation : AbstractValidator<Loja>
    {
        public LojaValidation()
        {
            RuleFor(loja => new CnpjValidoSpecification().IsSatisfiedBy(loja.CNPJ!))
                .Must(loja => loja)
                .WithMessage(LojaConstants.CnpjInvalido)
                .WithName(nameof(Loja.CNPJ));

            RuleFor(loja => new RazaoSocialSpecification().IsSatisfiedBy(loja))
                .Must(loja => loja)
                .WithMessage(LojaConstants.CnpjCadastrado)
                .WithName(nameof(Loja.CNPJ));

            RuleFor(loja => new EmailSpecifications().IsSatisfiedBy(loja))
                .Must(loja => loja)
                .WithMessage(LojaConstants.EmailInvalido)
                .WithName(nameof(Loja.Email));

            RuleFor(loja => new TelefoneSpecification().IsSatisfiedBy(loja))
                .Must(loja => loja)
                .WithMessage(LojaConstants.TelefoneInvalido)
                .WithName(nameof(Loja.Telefone));

            RuleFor(loja => loja)
                .Must(loja =>
                {
                    var campos = new List<KeyValuePair<string, int>>
                    {
                      new KeyValuePair<string, int>(loja.RazaoSocial ?? string.Empty, 250),
                      new KeyValuePair<string, int>(loja.Email ?? string.Empty, 100),
                      new KeyValuePair<string, int>(loja.Telefone ?? string.Empty, 20),
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
