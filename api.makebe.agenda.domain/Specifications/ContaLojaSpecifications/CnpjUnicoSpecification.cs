using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Specifications.SpecificationContext;
using api.makebe.agenda.infra.data.Repositorys.Interfaces;

namespace api.makebe.agenda.domain.Specifications.LojaSpecifications
{
    public class CnpjUnicoSpecification : Specification<ContaLoja>
    {
        private readonly IContaLojaRepository _lojaRepository;
        public CnpjUnicoSpecification(IContaLojaRepository lojaRepository)
        {
            _lojaRepository = lojaRepository;
        }

        public override bool IsSatisfiedBy(ContaLoja item)
        {
            var result = _lojaRepository.BuscarLojaPorCNPJ(item?.Cnpj ?? string.Empty, item?.ContaId ?? Guid.Empty).Result.Id > 0;
            return result;
        }
    }
}
