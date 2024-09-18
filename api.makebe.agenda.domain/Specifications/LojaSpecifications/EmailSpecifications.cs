using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Specifications.SpecificationContext;
using System.Text.RegularExpressions;

namespace api.makebe.agenda.domain.Specifications.LojaSpecifications
{
    public class EmailSpecifications : Specification<Loja>
    {
        public override bool IsSatisfiedBy(Loja item)
        {
            if (string.IsNullOrEmpty(item.Email))
                return false;
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(item.Email ?? string.Empty);
        }
    }
}
