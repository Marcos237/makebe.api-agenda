using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Specifications.SpecificationContext;

namespace api.makebe.agenda.domain.Specifications.EnderecoSpecifications
{
    public class EstadoSpecification : Specification<Endereco>
    {
        public override bool IsSatisfiedBy(Endereco item)
        {
            return !String.IsNullOrEmpty(item.Estado) && item?.Estado?.Length > 2 && item?.Estado?.Length < 100;
        }
    }
}
