using api.makebe.agenda.domain.Specifications.SpecificationContext;

namespace api.makebe.agenda.domain.Specifications.TextoSpecifications
{
    public class DataInicioMenorQueDataFimSpecification : Specification<(DateTime dataInicio, DateTime dataFim)>
    {
        public override bool IsSatisfiedBy((DateTime dataInicio, DateTime dataFim) datas)
        {
            return datas.dataInicio < datas.dataFim;
        }
    }
}
