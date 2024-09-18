namespace api.makebe.agenda.domain.Specifications.Interfaces
{
    public interface ISpecification<T>
    {
        bool IsSatisfiedBy(T item);
    }
}
