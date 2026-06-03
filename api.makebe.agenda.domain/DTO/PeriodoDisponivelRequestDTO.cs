namespace api.makebe.agenda.domain.DTO
{
    public class PeriodoDisponivelRequestDTO
    {
        public int IdServico { get; set; }
        public int IdColaborador { get; set; }
        public DateTime Data { get; set; }
    }
}
