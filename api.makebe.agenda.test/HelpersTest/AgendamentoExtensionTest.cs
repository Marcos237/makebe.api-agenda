using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Extensions;
using FluentAssertions;

namespace api.makebe.agenda.test.HelpersTest
{
    public class AgendamentoExtensionTest
    {
        [Fact]
        public void CalcularEhDesativado_DeveRetornarTrueParaAgendamentoFuturo()
        {
            var agendamento = new AgendamentoConsultaDTO
            {
                DataInicioAgendamento = DateTime.Now.AddMinutes(10)
            };

            agendamento.CalcularEhDesativado().Should().BeTrue();
        }

        [Fact]
        public void CalcularEhDesativado_DeveRetornarFalseParaAgendamentoAtualOuPassado()
        {
            var agendamento = new AgendamentoConsultaDTO
            {
                DataInicioAgendamento = DateTime.Now.AddMinutes(-10)
            };

            agendamento.CalcularEhDesativado().Should().BeFalse();
        }
    }
}
