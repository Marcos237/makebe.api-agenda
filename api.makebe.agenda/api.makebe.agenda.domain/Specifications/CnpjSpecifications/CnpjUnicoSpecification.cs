using api.makebe.agenda.domain.Specifications.SpecificationContext;
using api.makebe.agenda.domain.ValueObjects;

namespace api.makebe.agenda.domain.Specifications.CnpjSpecifications
{
    public class CnpjUnicoSpecification : Specification<CNPJ>
    {

        public override bool IsSatisfiedBy(CNPJ item)
        {
            return item.IsValidCNPJ(item?.Codigo ?? string.Empty);
        }
    }
}
