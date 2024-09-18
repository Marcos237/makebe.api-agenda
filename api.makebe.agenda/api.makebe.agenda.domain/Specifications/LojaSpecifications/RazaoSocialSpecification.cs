using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Specifications.SpecificationContext;

namespace api.makebe.agenda.domain.Specifications.LojaSpecifications
{
    public class RazaoSocialSpecification : Specification<Loja>
    {
        public override bool IsSatisfiedBy(Loja item)
        {
            return !String.IsNullOrEmpty(item.RazaoSocial) && item?.RazaoSocial?.Length > 3 && item?.RazaoSocial?.Length < 250;
        }
    }
}
