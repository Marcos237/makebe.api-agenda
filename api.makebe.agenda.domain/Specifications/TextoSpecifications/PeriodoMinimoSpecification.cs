using api.makebe.agenda.domain.Specifications.SpecificationContext;

namespace api.makebe.agenda.domain.Specifications.TextoSpecifications
{
    public class PeriodoMinimoSpecification : Specification<decimal>
    {
        public override bool IsSatisfiedBy(decimal item)
        {
            return item > 0.05M;
        }
    }
}
