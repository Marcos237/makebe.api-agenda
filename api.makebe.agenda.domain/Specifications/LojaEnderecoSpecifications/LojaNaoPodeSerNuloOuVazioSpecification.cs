using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Specifications.SpecificationContext;

namespace api.makebe.agenda.domain.Specifications.LojaEnderecoSpecifications
{
    public class LojaNaoPodeSerNuloOuVazioSpecification : Specification<LojaEndereco>
    {
        public override bool IsSatisfiedBy(LojaEndereco item)
        {
            return item.LojaId > 0;
        }
    }
}
