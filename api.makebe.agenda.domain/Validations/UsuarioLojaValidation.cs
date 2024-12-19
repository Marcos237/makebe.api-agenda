using api.makebe.agenda.domain.Constants;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Specifications.LojaSpecifications;
using api.makebe.agenda.infra.data.Repositorys.Interfaces;
using FluentValidation;

namespace api.makebe.agenda.domain.Validations
{
    public class UsuarioLojaValidation : AbstractValidator<ContaLoja>
    {
        private readonly IContaLojaRepository _usuarioLojaRepository;
        public UsuarioLojaValidation(IContaLojaRepository usuarioLojaRepository)
        {
            _usuarioLojaRepository = usuarioLojaRepository;

            RuleFor(loja => new CnpjUnicoSpecification(_usuarioLojaRepository).IsSatisfiedBy(loja))
                .Must(loja => loja)
                .WithMessage(LojaConstants.CnpjInvalido)
                .WithName(nameof(Loja.CNPJ));
        }
    }
}
