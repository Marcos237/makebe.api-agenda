using api.makebe.agenda.domain.Constants;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Specifications.SpecificationContext;

namespace api.makebe.agenda.domain.Specifications.ImagemSpecifications
{
    public class TamanhoImagemSpecification : Specification<Arquivo>
    {
        public override bool IsSatisfiedBy(Arquivo item)
        {
            byte[] imageBytes = System.Text.Encoding.UTF8.GetBytes(item?.UrlImagem ?? string.Empty);
            return imageBytes!.Any() && imageBytes!.Length > 0 && imageBytes.Length < ImagensConstants.TamanhoArquivo;
        }
    }
}
