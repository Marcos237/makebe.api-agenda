using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Extensions;
using api.makebe.agenda.domain.Services;
using FluentAssertions;
using System.Collections.Generic;
using System.Reflection;

namespace api.makebe.agenda.test.DomainServicesTest
{
    public class PeriodoDisponivelAgendamentoDomainServiceTest
    {
        private readonly PeriodoDisponivelAgendamentoDomainService _service = new();
        private readonly DateTime _dataBase = new(2026, 6, 2);

        private static List<PeriodoDTO> GerarSlots(DateTime inicio, DateTime fim, TimeSpan duracaoSlot, IEnumerable<DateTime> marcados)
        {
            var resultado = new List<PeriodoDTO>();
            for (var cur = inicio; cur < fim; cur = cur.Add(duracaoSlot))
            {
                resultado.Add(new PeriodoDTO
                {
                    Inicio = cur,
                    Fim = cur.Add(duracaoSlot),
                    IsAgendado = marcados != null && marcados.Contains(cur)
                });
            }
            return resultado;
        }

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

        [Fact]
        public void ReagruparPeriodosMarcados_DeveReagruparPeriodoLongo()
        {
            // Arrange
            decimal periodoServico = 1;
            var duracaoSlot = periodoServico.ParaTimeSpan();

            var agendas = new List<AgendamentoColaboradorPeriodoDTO>
    {
        new AgendamentoColaboradorPeriodoDTO
        {
            IdAgendaColaborador = 1,
            ColaboradorId = 1,
            DataInicioAgendamento = new DateTime(2026, 06, 06, 08, 00, 00),
            DataTerminoAgendamento = new DateTime(2026, 06, 06, 08, 10, 00),
            Periodo = 0.10m
        }
    };

            var disponiveis = new List<(DateTime Inicio, DateTime Fim)>
    {
        (new DateTime(2026, 06, 06, 08, 00, 00), new DateTime(2026, 06, 06, 12, 00, 00)),
        (new DateTime(2026, 06, 06, 13, 00, 00), new DateTime(2026, 06, 06, 20, 00, 00)),

    };

            // Act
            var response = _service.ReagruparPeriodosMarcar(
                disponiveis,
                agendas,
                duracaoSlot);

            Assert.Equal(11, response.Count);

            var esperado = new[]
            {
    (new DateTime(2026, 06, 06, 08, 00, 00), new DateTime(2026, 06, 06, 08, 10, 00), true),
    (new DateTime(2026, 06, 06, 08, 10, 00), new DateTime(2026, 06, 06, 09, 10, 00), false),
    (new DateTime(2026, 06, 06, 09, 10, 00), new DateTime(2026, 06, 06, 10, 10, 00), false),
    (new DateTime(2026, 06, 06, 10, 10, 00), new DateTime(2026, 06, 06, 11, 10, 00), false),

    (new DateTime(2026, 06, 06, 13, 00, 00), new DateTime(2026, 06, 06, 14, 00, 00), false),
    (new DateTime(2026, 06, 06, 14, 00, 00), new DateTime(2026, 06, 06, 15, 00, 00), false),
    (new DateTime(2026, 06, 06, 15, 00, 00), new DateTime(2026, 06, 06, 16, 00, 00), false),
    (new DateTime(2026, 06, 06, 16, 00, 00), new DateTime(2026, 06, 06, 17, 00, 00), false),
    (new DateTime(2026, 06, 06, 17, 00, 00), new DateTime(2026, 06, 06, 18, 00, 00), false),
    (new DateTime(2026, 06, 06, 18, 00, 00), new DateTime(2026, 06, 06, 19, 00, 00), false),
    (new DateTime(2026, 06, 06, 19, 00, 00), new DateTime(2026, 06, 06, 20, 00, 00), false)
};

            for (var i = 0; i < esperado.Length; i++)
            {
                Assert.Equal(esperado[i].Item1, response[i].Inicio);
                Assert.Equal(esperado[i].Item2, response[i].Fim);
                Assert.Equal(esperado[i].Item3, response[i].IsAgendado);
            }
        }

        [Fact]
        public void ReagruparPeriodosMarcados_SemAgendamentos_DeveManterPeriodosOriginais()
        {
            // Arrange
            var duracaoSlot = TimeSpan.FromHours(1);

            var agendas = new List<AgendamentoColaboradorPeriodoDTO>();

            var disponiveis = new List<(DateTime Inicio, DateTime Fim)>
    {
        (new DateTime(2026, 06, 06, 08, 00, 00), new DateTime(2026, 06, 06, 12, 00, 00))

    };

            // Act
            var response = _service.ReagruparPeriodosMarcar(
                disponiveis,
                agendas,
                duracaoSlot);

            // Assert
            var esperado = new[]
            {
        (new DateTime(2026, 06, 06, 08, 00, 00), new DateTime(2026, 06, 06, 09, 00, 00)),
        (new DateTime(2026, 06, 06, 09, 00, 00), new DateTime(2026, 06, 06, 10, 00, 00)),
        (new DateTime(2026, 06, 06, 10, 00, 00), new DateTime(2026, 06, 06, 11, 00, 00)),
        (new DateTime(2026, 06, 06, 11, 00, 00), new DateTime(2026, 06, 06, 12, 00, 00))
    };

            Assert.Equal(esperado.Length, response.Count);

            for (var i = 0; i < esperado.Length; i++)
            {
                Assert.False(response[i].IsAgendado);
                Assert.Equal(esperado[i].Item1, response[i].Inicio);
                Assert.Equal(esperado[i].Item2, response[i].Fim);
            }
        }

        [Fact]
        public void ReagruparPeriodosMarcados_DeveReagruparComMarcacoesManhaETarde()
        {
            // Arrange
            var duracaoSlot = TimeSpan.FromMinutes(40);

            var agendas = new List<AgendamentoColaboradorPeriodoDTO>
    {
        // Manhã
        new AgendamentoColaboradorPeriodoDTO
        {
            IdAgendaColaborador = 1,
            ColaboradorId = 1,
            DataInicioAgendamento = new DateTime(2026, 06, 06, 08, 00, 00),
            DataTerminoAgendamento = new DateTime(2026, 06, 06, 08, 15, 00),
            Periodo = 0.15m
        },

        // Tarde
        new AgendamentoColaboradorPeriodoDTO
        {
            IdAgendaColaborador = 2,
            ColaboradorId = 1,
            DataInicioAgendamento = new DateTime(2026, 06, 06, 13, 40, 00),
            DataTerminoAgendamento = new DateTime(2026, 06, 06, 13, 50, 00),
            Periodo = 0.10m
        },
        new AgendamentoColaboradorPeriodoDTO
        {
            IdAgendaColaborador = 3,
            ColaboradorId = 1,
            DataInicioAgendamento = new DateTime(2026, 06, 06, 14, 20, 00),
            DataTerminoAgendamento = new DateTime(2026, 06, 06, 14, 50, 00),
            Periodo = 0.30m
        }
    };

            var disponiveis = new List<(DateTime Inicio, DateTime Fim)>
    {
        (new DateTime(2026, 06, 06, 08, 00, 00), new DateTime(2026, 06, 06, 12, 00, 00)),
        (new DateTime(2026, 06, 06, 13, 00, 00), new DateTime(2026, 06, 06, 20, 00, 00))
    };

            // Act
            var response = _service.ReagruparPeriodosMarcar(
                disponiveis,
                agendas,
                duracaoSlot);

            // Assert

            // Quantidade retornada atualmente
            Assert.Equal(17, response.Count);

            // Deve haver períodos agendados
            Assert.Contains(response, x => x.IsAgendado);

            // Primeiro período deve começar às 08:00
            Assert.Equal(
                new DateTime(2026, 06, 06, 08, 00, 00),
                response.First().Inicio);

            // Deve existir marcação na manhã
            Assert.Contains(response,
                x => x.IsAgendado &&
                     x.Inicio >= new DateTime(2026, 06, 06, 08, 00, 00) &&
                     x.Inicio < new DateTime(2026, 06, 06, 12, 00, 00));

            // Deve existir marcação na tarde
            Assert.Contains(response,
                x => x.IsAgendado &&
                     x.Inicio >= new DateTime(2026, 06, 06, 13, 00, 00) &&
                     x.Inicio < new DateTime(2026, 06, 06, 20, 00, 00));

            // Todos os períodos devem ser válidos
            Assert.All(response, periodo =>
            {
                Assert.True(periodo.Inicio < periodo.Fim);
            });
        }
        [Fact]
        public void ReagruparPeriodosMarcados_TodosMarcados_NaoDeveRemarcar()
        {
            // Arrange
            var duracaoSlot = TimeSpan.FromMinutes(40);

            var agendas = new List<AgendamentoColaboradorPeriodoDTO>
    {
        new AgendamentoColaboradorPeriodoDTO
        {
            DataInicioAgendamento = new DateTime(2026, 06, 06, 08, 00, 00),
            DataTerminoAgendamento = new DateTime(2026, 06, 06, 08, 40, 00),
            Periodo = 0.40m
        },
        new AgendamentoColaboradorPeriodoDTO
        {
            DataInicioAgendamento = new DateTime(2026, 06, 06, 08, 40, 00),
            DataTerminoAgendamento = new DateTime(2026, 06, 06, 09, 20, 00),
            Periodo = 0.40m
        },
        new AgendamentoColaboradorPeriodoDTO
        {
            DataInicioAgendamento = new DateTime(2026, 06, 06, 09, 20, 00),
            DataTerminoAgendamento = new DateTime(2026, 06, 06, 10, 00, 00),
            Periodo = 0.40m
        },
        new AgendamentoColaboradorPeriodoDTO
        {
            DataInicioAgendamento = new DateTime(2026, 06, 06, 10, 00, 00),
            DataTerminoAgendamento = new DateTime(2026, 06, 06, 10, 40, 00),
            Periodo = 0.40m
        },
        new AgendamentoColaboradorPeriodoDTO
        {
            DataInicioAgendamento = new DateTime(2026, 06, 06, 10, 40, 00),
            DataTerminoAgendamento = new DateTime(2026, 06, 06, 11, 20, 00),
            Periodo = 0.40m
        },
        new AgendamentoColaboradorPeriodoDTO
        {
            DataInicioAgendamento = new DateTime(2026, 06, 06, 11, 20, 00),
            DataTerminoAgendamento = new DateTime(2026, 06, 06, 12, 00, 00),
            Periodo = 0.40m
        }
    };

            var disponiveis = new List<(DateTime Inicio, DateTime Fim)>
    {
        (new DateTime(2026, 06, 06, 08, 00, 00), new DateTime(2026, 06, 06, 12, 00, 00))
    };

            // Act
            var response = _service.ReagruparPeriodosMarcar(
                disponiveis,
                agendas,
                duracaoSlot);

            // Assert
            Assert.Equal(6, response.Count);

            Assert.All(response, periodo =>
            {
                Assert.True(periodo.IsAgendado);
            });

            var esperado = new[]
            {
        (new DateTime(2026, 06, 06, 08, 00, 00), new DateTime(2026, 06, 06, 08, 40, 00)),
        (new DateTime(2026, 06, 06, 08, 40, 00), new DateTime(2026, 06, 06, 09, 20, 00)),
        (new DateTime(2026, 06, 06, 09, 20, 00), new DateTime(2026, 06, 06, 10, 00, 00)),
        (new DateTime(2026, 06, 06, 10, 00, 00), new DateTime(2026, 06, 06, 10, 40, 00)),
        (new DateTime(2026, 06, 06, 10, 40, 00), new DateTime(2026, 06, 06, 11, 20, 00)),
        (new DateTime(2026, 06, 06, 11, 20, 00), new DateTime(2026, 06, 06, 12, 00, 00))
    };

            for (var i = 0; i < esperado.Length; i++)
            {
                Assert.Equal(esperado[i].Item1, response[i].Inicio);
                Assert.Equal(esperado[i].Item2, response[i].Fim);
            }
        }
    }
}
