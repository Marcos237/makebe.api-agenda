using api.makebe.agenda.domain.Specifications.SpecificationContext;

namespace api.makebe.agenda.domain.Specifications.TextoSpecifications
{
    public class DataValidaSpecification : Specification<DateTime?>
    {
        public override bool IsSatisfiedBy(DateTime? data)
        {
            return data.HasValue && data.Value != DateTime.MinValue;
        }
    }
}