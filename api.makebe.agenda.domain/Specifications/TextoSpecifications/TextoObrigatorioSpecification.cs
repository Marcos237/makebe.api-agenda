using api.makebe.agenda.domain.Specifications.SpecificationContext;
namespace api.makebe.agenda.domain.Specifications.TextoSpecifications
{
    public class TextoObrigatorioSpecification : Specification<string>
    {
        public override bool IsSatisfiedBy(string item)
        {
            return !String.IsNullOrEmpty(item);
        }
    }
}
