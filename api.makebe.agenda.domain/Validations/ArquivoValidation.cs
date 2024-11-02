using api.makebe.agenda.domain.Constants;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Specifications.ImagemSpecifications;
using api.makebe.agenda.domain.Specifications.TextoSpecifications;
using FluentValidation;

namespace api.makebe.agenda.domain.Validations
{
    public class ArquivoValidation : AbstractValidator<Arquivo>
    {
        public ArquivoValidation()
        {
            RuleFor(imagem => new TextoObrigatorioSpecification().IsSatisfiedBy(imagem!.UrlImagem!)).Must(imagem =>
            {
                return imagem;
            }).WithMessage(ImagensConstants.ImagemInvalido);

            RuleFor(imagem => new TextoObrigatorioSpecification().IsSatisfiedBy(imagem!.NomeArquivo!)).Must(imagem =>
            {
                return imagem;
            }).WithMessage(ImagensConstants.ImagemInvalido);

            RuleFor(imagem => new TamanhoImagemSpecification().IsSatisfiedBy(imagem!)).Must(imagem =>
            {
                return imagem;
            }).WithMessage(ImagensConstants.ImagemInvalido);

            RuleFor(imagem => new TipoImagemSpecification().IsSatisfiedBy(imagem!)).Must(imagem =>
            {
                return imagem;
            }).WithMessage(ImagensConstants.ImagemInvalido);

            RuleFor(imagem => imagem)
              .Must(imagem =>
              {
                  var campos = new List<KeyValuePair<string, int>>
                  {
                      new KeyValuePair<string, int>(imagem!.UrlImagem ?? string.Empty, 250),
                      new KeyValuePair<string, int>(imagem.NomeArquivo ?? string.Empty, 250),
                      new KeyValuePair<string, int>(imagem.TituloImagem ?? string.Empty, 100),
                  }
                  .Where(kvp => !string.IsNullOrWhiteSpace(kvp.Key))
                  .DistinctBy(kvp => kvp.Key)
                  .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

                  return new TamanhoCamposSpecification().IsSatisfiedBy(campos);
              })
              .WithMessage(BaseConstant.Campos);
        }
    }
}
