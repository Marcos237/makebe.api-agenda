using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Specifications.SpecificationContext;
using System.Text.RegularExpressions;

namespace api.makebe.agenda.domain.Specifications.EnderecoSpecifications
{
    public class CepSpecification : Specification<Endereco>
    {
        public override bool IsSatisfiedBy(Endereco item)
        {
            return !String.IsNullOrEmpty(item.CEP) && item.CEP.Length > 6;
        }
    }
}
