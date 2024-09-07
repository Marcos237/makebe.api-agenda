using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Specifications.SpecificationContext;

namespace api.makebe.agenda.domain.Specifications.EnderecoSpecifications
{
    internal class LogradouroSpecifications : Specification<Endereco>
    {
        public override bool IsSatisfiedBy(Endereco item)
        {
            return !String.IsNullOrEmpty(item.Logradouro) && item?.Logradouro?.Length > 3 && item?.Logradouro?.Length < 250;
        }
    }
}
