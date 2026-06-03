using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Services;
using FluentAssertions;

namespace api.makebe.agenda.test.DomainServicesTest
{
    public class PeriodoDisponivelAgendamentoDomainServiceTest
    {
        private readonly PeriodoDisponivelAgendamentoDomainService _service = new();
        private readonly DateTime _dataBase = new(2026, 6, 2);

        [Fact]
        public async Task MontarPeriodosDisponiveis_DeveGerarPeriodosCorretamenteQuandoNaoHouverBloqueios()
        {
            var response = await _service.MontarPeriodosDisponiveis(_dataBase, 30, Enumerable.Empty<AgendamentoColaboradorPeriodoDTO>());

            response.Should().HaveCount(48);
            response.First().Inicio.Should().Be(_dataBase);
            response.First().Fim.Should().Be(_dataBase.AddMinutes(30));
            response.Last().Inicio.Should().Be(_dataBase.AddHours(23).AddMinutes(30));
            response.Last().Fim.Should().Be(_dataBase.AddDays(1));
        }

        [Theory]
        [InlineData(15, 96)]
        [InlineData(30, 48)]
        [InlineData(45, 32)]
        [InlineData(60, 24)]
        public async Task MontarPeriodosDisponiveis_DeveRespeitarValorDoPeriodo(decimal periodo, int quantidadeEsperada)
        {
            var response = await _service.MontarPeriodosDisponiveis(_dataBase, periodo, Enumerable.Empty<AgendamentoColaboradorPeriodoDTO>());

            response.Should().HaveCount(quantidadeEsperada);
        }

        [Fact]
        public async Task MontarPeriodosDisponiveis_DeveRemoverHorariosDentroDoPeriodoInativo()
        {
            var agendas = new[]
            {
                CriarAgenda(periodoInativoInicio: _dataBase.AddHours(20), periodoInativoFim: _dataBase.AddDays(1).AddHours(8))
            };

            var response = await _service.MontarPeriodosDisponiveis(_dataBase, 60, agendas);

            response.Should().HaveCount(12);
            response.First().Inicio.Should().Be(_dataBase.AddHours(8));
            response.Last().Inicio.Should().Be(_dataBase.AddHours(19));
        }

        [Fact]
        public async Task MontarPeriodosDisponiveis_DeveRemoverHorariosDentroDaAgendaBloqueada()
        {
            var agendas = new[]
            {
                CriarAgenda(agendaBloqueadaInicio: _dataBase.AddHours(12), agendaBloqueadaFim: _dataBase.AddHours(13))
            };

            var response = await _service.MontarPeriodosDisponiveis(_dataBase, 30, agendas);

            response.Should().NotContain(periodo => periodo.Inicio == _dataBase.AddHours(12));
            response.Should().NotContain(periodo => periodo.Inicio == _dataBase.AddHours(12).AddMinutes(30));
            response.Should().Contain(periodo => periodo.Inicio == _dataBase.AddHours(13));
        }

        [Fact]
        public async Task MontarPeriodosDisponiveis_DeveTratarMultiplosBloqueiosESobreposicoesSemDuplicar()
        {
            var agendas = new[]
            {
                CriarAgenda(periodoInativoInicio: _dataBase.AddHours(20), periodoInativoFim: _dataBase.AddDays(1).AddHours(8)),
                CriarAgenda(agendaBloqueadaInicio: _dataBase.AddHours(10), agendaBloqueadaFim: _dataBase.AddHours(12)),
                CriarAgenda(agendaBloqueadaInicio: _dataBase.AddHours(11), agendaBloqueadaFim: _dataBase.AddHours(13))
            };

            var response = await _service.MontarPeriodosDisponiveis(_dataBase, 60, agendas);

            response.Should().OnlyHaveUniqueItems(periodo => new { periodo.Inicio, periodo.Fim });
            response.Should().BeInAscendingOrder(periodo => periodo.Inicio);
            response.Should().NotContain(periodo => periodo.Inicio >= _dataBase.AddHours(10) && periodo.Inicio < _dataBase.AddHours(13));
        }

        [Fact]
        public async Task MontarPeriodosDisponiveis_DeveMarcarIsAgendadoQuandoExistirAgendamentoCorrespondente()
        {
            var agendas = new[]
            {
                CriarAgenda(dataInicioAgendamento: _dataBase.AddHours(10), dataTerminoAgendamento: _dataBase.AddHours(10).AddMinutes(29))
            };

            var response = (await _service.MontarPeriodosDisponiveis(_dataBase, 30, agendas)).ToList();

            response.Should().Contain(periodo => periodo.Inicio == _dataBase.AddHours(10) && periodo.IsAgendado);
            response.Should().Contain(periodo => periodo.Inicio == _dataBase.AddHours(10).AddMinutes(30) && !periodo.IsAgendado);
        }

        [Fact]
        public async Task MontarPeriodosDisponiveis_DeveTratarMultiplosAgendamentosEIgnorarRegistrosSemDataInicio()
        {
            var agendas = new[]
            {
                CriarAgenda(dataInicioAgendamento: _dataBase.AddHours(9), dataTerminoAgendamento: _dataBase.AddHours(9).AddMinutes(29)),
                CriarAgenda(dataInicioAgendamento: _dataBase.AddHours(15), dataTerminoAgendamento: _dataBase.AddHours(15).AddMinutes(29)),
                CriarAgenda(dataInicioAgendamento: null, dataTerminoAgendamento: _dataBase.AddHours(18))
            };

            var response = (await _service.MontarPeriodosDisponiveis(_dataBase, 30, agendas)).ToList();

            response.Count(periodo => periodo.IsAgendado).Should().Be(2);
            response.Should().Contain(periodo => periodo.Inicio == _dataBase.AddHours(9) && periodo.IsAgendado);
            response.Should().Contain(periodo => periodo.Inicio == _dataBase.AddHours(15) && periodo.IsAgendado);
        }

        [Fact]
        public async Task MontarPeriodosDisponiveis_DeveRetornarListaVaziaQuandoNaoHouverPeriodosDisponiveis()
        {
            var agendas = new[]
            {
                CriarAgenda(periodoInativoInicio: _dataBase, periodoInativoFim: _dataBase)
            };

            var response = await _service.MontarPeriodosDisponiveis(_dataBase, 30, agendas);

            response.Should().BeEmpty();
        }

        private AgendamentoColaboradorPeriodoDTO CriarAgenda(
            DateTime? periodoInativoInicio = null,
            DateTime? periodoInativoFim = null,
            DateTime? agendaBloqueadaInicio = null,
            DateTime? agendaBloqueadaFim = null,
            DateTime? dataInicioAgendamento = null,
            DateTime? dataTerminoAgendamento = null)
        {
            return new AgendamentoColaboradorPeriodoDTO
            {
                IdAgendaColaborador = 1,
                ColaboradorId = 1,
                PeriodoInativoInicio = periodoInativoInicio ?? default,
                PeriodoInativoFim = periodoInativoFim ?? default,
                AgendaBloqueadaInicio = agendaBloqueadaInicio ?? default,
                AgendaBloqueadaFim = agendaBloqueadaFim ?? default,
                DataInicioAgendamento = dataInicioAgendamento,
                DataTerminoAgendamento = dataTerminoAgendamento
            };
        }
    }
}
