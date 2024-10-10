using api.makebe.agenda.domain.Specifications.SpecificationContext;

namespace api.makebe.agenda.domain.Specifications.TextoSpecifications
{
    public class TamanhoCamposSpecification : Specification<Dictionary<string, int>>
    {
        public override bool IsSatisfiedBy(Dictionary<string, int> items)
        {
            var retorno = false;
            if (!items.Any())
                return false;

            foreach (var item in items)
            {
                retorno = item.Key.Length <= item.Value;
            }
            return retorno;
        }
    }
}
