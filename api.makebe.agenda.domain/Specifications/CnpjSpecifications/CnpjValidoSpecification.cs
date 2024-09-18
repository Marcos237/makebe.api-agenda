using api.makebe.agenda.domain.Specifications.SpecificationContext;
using api.makebe.agenda.domain.ValueObjects;

namespace api.makebe.agenda.domain.Specifications.CnpjSpecifications
{
    public class CnpjValidoSpecification : Specification<CNPJ>
    {
        public override bool IsSatisfiedBy(CNPJ item)
        {
            return item.IsValidCNPJ(item?.Codigo ?? string.Empty);
        }
    }
}
