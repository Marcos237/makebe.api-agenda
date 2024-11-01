using api.makebe.agenda.domain.Constants;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Specifications.SpecificationContext;

namespace api.makebe.agenda.domain.Specifications.ImagemSpecifications
{
    public class TipoImagemSpecification : Specification<Arquivo>
    {
        public override bool IsSatisfiedBy(Arquivo item)
        {
            var extensao = Path.GetExtension(item.TipoArquivo)?.ToUpper();
            return extensao == ImagensConstants.ExtensaoJpeg || extensao == ImagensConstants.ExtensaoJpg ||
                extensao == ImagensConstants.ExtensaoPng;
        }
    }
}
