using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Specifications.SpecificationContext;

namespace api.makebe.agenda.domain.Specifications.EnderecoSpecifications
{
    public class CidadeSpecification : Specification<Endereco>
    {
        public override bool IsSatisfiedBy(Endereco item)
        {
            return !String.IsNullOrEmpty(item.Cidade) && item?.Cidade.Length > 2 && item?.Cidade?.Length < 250;
        }
    }
}
