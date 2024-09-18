using api.makebe.agenda.domain.Specifications.Interfaces;

namespace api.makebe.agenda.domain.Specifications.SpecificationContext
{
    public abstract class Specification<T> : ISpecification<T>
    {
        public abstract bool IsSatisfiedBy(T item);
    }
}
