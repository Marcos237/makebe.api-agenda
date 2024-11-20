using api.makebe.agenda.domain.Specifications.SpecificationContext;

namespace api.makebe.agenda.domain.Specifications.LojaSpecifications
{
    public class LojaNaoPodeSerNuloOuVazioSpecification : Specification<int>
    {
        public override bool IsSatisfiedBy(int id)
        {
            return id > 0;
        }
    }
}
