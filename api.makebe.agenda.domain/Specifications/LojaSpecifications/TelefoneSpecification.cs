using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Specifications.SpecificationContext;

namespace api.makebe.agenda.domain.Specifications.LojaSpecifications
{
    public class TelefoneSpecification : Specification<Loja>
    {
        public override bool IsSatisfiedBy(Loja item)
        {
            return !String.IsNullOrEmpty(item.Telefone) && item.Telefone.Length > 9;
        }
    }
}
