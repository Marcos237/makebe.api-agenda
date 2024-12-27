using api.makebe.agenda.domain.Specifications.SpecificationContext;

namespace api.makebe.agenda.domain.Specifications.TextoSpecifications
{
    internal class IdsObrigatoriosSpecifications : Specification<int>
    {
        public override bool IsSatisfiedBy(int id)
        {
            return id > 0;
        }
    }
}