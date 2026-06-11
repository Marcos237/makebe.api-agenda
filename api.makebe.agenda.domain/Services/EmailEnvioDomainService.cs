using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Helpers;
using api.makebe.agenda.domain.Interfaces.Repositorys;
using api.makebe.agenda.domain.Interfaces.Services;
using api.makebe.agenda.infra.crosscutting.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using UsuariosEvent;

namespace api.makebe.agenda.domain.Services
{
    public class EmailEnvioDomainService : IEmailEnvioDomainService
    {
        private const string PastaAgendamento = "agendamento";
        private const string Sistema = "MakeBe";
        private const string UsuarioSistema = "Sistema";
        private readonly IEmailEnvioRepository _emailEnvioRepository;
        private readonly IUsuarioEventCrossCuttingService _usuarioEventCrossCuttingService;
        private readonly IColaboradorDomainService _colaboradorDomainService;
        private readonly string _urlMakebe;

        public EmailEnvioDomainService(
            IEmailEnvioRepository emailEnvioRepository,
            IUsuarioEventCrossCuttingService usuarioEventCrossCuttingService,
            IColaboradorDomainService colaboradorDomainService,
            IConfiguration configuration)
        {
            _emailEnvioRepository = emailEnvioRepository;
            _usuarioEventCrossCuttingService = usuarioEventCrossCuttingService;
            _colaboradorDomainService = colaboradorDomainService;
            _urlMakebe = configuration["urlMakebe"] ?? string.Empty;
        }

        public async Task GerarEmailsAgendamento(AgendamentoDTO agendamentoDTO)
        {
            var dadosCliente = await BuscarDadosUsuario(agendamentoDTO.IdUsuario);
            var dadosProfissional = await BuscarDadosProfissional(agendamentoDTO.IdColaborador);

            var emails = new List<EmailEnvio>();

            if (!string.IsNullOrWhiteSpace(dadosCliente.Email))
                emails.Add(CriarEmailCliente(agendamentoDTO, dadosCliente.Nome, dadosCliente.Email));

            if (!string.IsNullOrWhiteSpace(dadosProfissional.Email))
                emails.Add(CriarEmailProfissional(agendamentoDTO, dadosProfissional.Nome, dadosProfissional.Email, dadosCliente.Nome));

            foreach (var email in emails)
                await _emailEnvioRepository.Salvar(email);
        }

        private async Task<(string Nome, string Email)> BuscarDadosUsuario(string? idUsuario)
        {
            var usuarioEvent = new UsuarioConsultadoPorIdEvent()
            {
                Id = PropiedadesHelper.ParseGuidOrDefault(idUsuario)
            };

            var responseEvent =
                await _usuarioEventCrossCuttingService
                    .BuscarUsuarioPorId(usuarioEvent);

            var nome =
                responseEvent.UsuarioConsultadoRetorno?.Nome ?? string.Empty;

            var email = responseEvent.UsuarioConsultadoRetorno?.Email ?? string.Empty;

            return (nome, email);
        }

        private async Task<(string Nome, string Email)> BuscarDadosProfissional(string? idColaborador)
        {
            var colaborador = await _colaboradorDomainService.BuscarColaboradorPorId(Convert.ToInt32(TextoHelper.GetNumeros(idColaborador ?? "0")));
            var usuarioId = colaborador.UsuarioCodigo ?? colaborador.UsuarioId?.ToString() ?? string.Empty;
            return await BuscarDadosUsuario(usuarioId);
        }

        private EmailEnvio CriarEmailCliente(AgendamentoDTO agendamentoDTO, string nomeCliente, string emailCliente)
        {
            var dadosModelo = new
            {
                Nome = nomeCliente,
                Servico = agendamentoDTO.DescricaoServico ?? string.Empty,
                DataAgendamento = agendamentoDTO.DataInicioAgendamento.ToString("dd/MM/yyyy"),
                Horario = agendamentoDTO.DataInicioAgendamento.ToString("HH:mm"),
                LinkAgendamento = $"{_urlMakebe}/MeusAgendamentos"
            };

            return CriarEmailEnvio(
                JsonConvert.SerializeObject(dadosModelo),
                JsonConvert.SerializeObject(CriarDadosEnvio(emailCliente, nomeCliente, "Agendamento Confirmado")),
                "email-agendamento-cliente.html");
        }

        private EmailEnvio CriarEmailProfissional(AgendamentoDTO agendamentoDTO, string nomeProfissional, string emailProfissional, string nomeCliente)
        {
            var dadosModelo = new
            {
                Nome = nomeProfissional,
                NomeCliente = nomeCliente,
                Servico = agendamentoDTO.DescricaoServico ?? string.Empty,
                DataAgendamento = agendamentoDTO.DataInicioAgendamento.ToString("dd/MM/yyyy"),
                Horario = agendamentoDTO.DataInicioAgendamento.ToString("HH:mm"),
                LinkAgendamento = $"{_urlMakebe}/Agendamentos"
            };

            return CriarEmailEnvio(
                JsonConvert.SerializeObject(dadosModelo),
                JsonConvert.SerializeObject(CriarDadosEnvio(emailProfissional, nomeProfissional, "Novo Agendamento")),
                "email-agendamento.html");
        }

        private static object CriarDadosEnvio(string emailDestino, string nomeDestino, string assunto)
        {
            return new
            {
                Para = new[]
                {
                    new
                    {
                        Email = emailDestino,
                        Nome = nomeDestino
                    }
                },
                Copias = Array.Empty<object>(),
                Assunto = assunto,
                Sistema,
                Usuario = UsuarioSistema
            };
        }

        private static EmailEnvio CriarEmailEnvio(string dadosModelo, string dadosEnvio, string nomeArquivo)
        {
            return new EmailEnvio
            {
                DadosModelo = dadosModelo,
                DadosEnvio = dadosEnvio,
                Pasta = PastaAgendamento,
                NomeArquivo = nomeArquivo,
                DataCadastro = DateTime.Now,
                Processado = false,
                Tentativas = 0,
                DataProcessamento = null,
                Erro = null
            };
        }
    }
}
