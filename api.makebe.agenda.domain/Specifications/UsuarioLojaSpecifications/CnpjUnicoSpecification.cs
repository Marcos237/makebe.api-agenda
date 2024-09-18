using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Specifications.SpecificationContext;
using api.makebe.agenda.infra.data.Repositorys.Interfaces;

namespace api.makebe.agenda.domain.Specifications.LojaSpecifications
{
    public class CnpjUnicoSpecification : Specification<UsuarioLoja>
    {
        private readonly IUsuarioLojaRepository _lojaRepository;
        public CnpjUnicoSpecification(IUsuarioLojaRepository lojaRepository)
        {
            _lojaRepository = lojaRepository;
        }

        public override bool IsSatisfiedBy(UsuarioLoja item)
        {
            var result = _lojaRepository.BuscarLojaPorCNPJ(item?.Cnpj ?? string.Empty, item?.UsuarioId ?? Guid.Empty).Result.Id > 0;
            return result;
        }
    }
}
