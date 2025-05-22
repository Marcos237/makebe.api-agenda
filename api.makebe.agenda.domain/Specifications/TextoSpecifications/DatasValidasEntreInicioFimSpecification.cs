using api.makebe.agenda.domain.Specifications.SpecificationContext;

namespace api.makebe.agenda.domain.Specifications.TextoSpecifications
{
    public class DatasValidasEntreInicioFimSpecification : Specification<(DateTime? dataInicio, DateTime? dataFim)>
    {
        public override bool IsSatisfiedBy((DateTime? dataInicio, DateTime? dataFim) datas)
        {
            if (!datas.dataInicio.HasValue || !datas.dataFim.HasValue)
                return false;

            var inicio = datas.dataInicio.Value;
            var fim = datas.dataFim.Value;

            if (fim.TimeOfDay == TimeSpan.Zero)
                fim = fim.Date.AddDays(1).AddSeconds(-1);

            return inicio <= fim;
        }
    }
}

