using api.makebe.agenda.domain.Constants;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Specifications.ImagemSpecifications;
using api.makebe.agenda.domain.Specifications.TextoSpecifications;
using FluentValidation;
using static System.Net.Mime.MediaTypeNames;

namespace api.makebe.agenda.domain.Validations
{
    public class ArquivoValidation : AbstractValidator<Arquivo>
    {
        public ArquivoValidation()
        {
            RuleFor(imagem => imagem.UrlImagem)
                .Must(url => new TextoObrigatorioSpecification().IsSatisfiedBy(url!))
                .WithMessage(ImagensConstants.ImagemInvalido)
                .WithName(imagem => imagem.Id ?? "Imagem");


            RuleFor(imagem => imagem.NomeArquivo)
                .Must(nome => new TextoObrigatorioSpecification().IsSatisfiedBy(nome!))
                .WithMessage(ImagensConstants.ImagemInvalido)
                .WithName(imagem => imagem.Id ?? ImagensConstants.CampoImagem);

            RuleFor(imagem => imagem)
                .Must(imagem => new TamanhoImagemSpecification().IsSatisfiedBy(imagem!))
                .WithMessage(ImagensConstants.ImagemInvalido)
                .WithName(imagem => imagem.Id ?? ImagensConstants.CampoImagem);

            RuleFor(imagem => imagem)
                .Must(imagem => new TipoImagemSpecification().IsSatisfiedBy(imagem!))
                .WithMessage(ImagensConstants.ImagemInvalido)
                .WithName(imagem => imagem.Id ?? ImagensConstants.CampoImagem);

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
              .WithMessage(BaseConstant.Campos)
              .WithName(ImagensConstants.CampoImagem);
        }
    }
}
