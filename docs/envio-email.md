Objetivo:

Após salvar um agendamento com sucesso, devem ser gerados dois registros na tabela EMAIL.EmailEnvio:

1 - E-mail para o cliente.
2 - E-mail para o profissional.

Arquitetura:

Não adicionar lógica de montagem de e-mail dentro do AgendamentoApplicationService.

Criar:

- IEmailEnvioDomainService
- EmailEnvioDomainService
- IEmailEnvioRepository
- EmailEnvioRepository
- Entidade EmailEnvio

O Application Service deve apenas chamar:

await _emailEnvioDomainService.GerarEmailsAgendamento(agendamentoDTO);

Responsabilidades do domínio:

- Buscar nomes e email necessários através dos consumers.
- Montar DadosModelo.
- Montar DadosEnvio.
- Persistir utilizando EmailEnvioRepository.

---------------------------------------------------------
E-MAIL DO CLIENTE
---------------------------------------------------------

Template:

email-agendamento-cliente.html

Pasta:

agendamento

Variáveis:

@Nome
@Servico
@DataAgendamento
@Horario
@LinkAgendamento

Nome:

Buscar utilizando:

var usuarioEvent =
    new UsuarioConsultadoPorIdEvent()
    {
        Id = PropiedadesHelper.ParseGuidOrDefault(agendamentoDTO.IdUsuario)
    };

var responseEvent =
    await _usuarioEventCrossCuttingService
        .BuscarUsuarioPorId(usuarioEvent);

var nomeCliente =
    responseEvent.UsuarioConsultadoRetorno?.Nome ?? string.Empty;

Serviço:

Utilizar:

agendamentoDTO.DescricaoServico

Data:

agendamentoDTO.DataInicioAgendamento.ToString("dd/MM/yyyy")

Horário:

agendamentoDTO.DataInicioAgendamento.ToString("HH:mm")

Link:

appsettings:

UrlMakebe + "/MeusAgendamentos"

Assunto:

Agendamento Confirmado

---------------------------------------------------------
E-MAIL DO PROFISSIONAL
---------------------------------------------------------

Template:

email-agendamento.html

Pasta:

agendamento

Variáveis:

@Nome
@NomeCliente
@Servico
@DataAgendamento
@Horario
@LinkAgendamento

Nome do profissional:

Utilizar o fluxo já existente do colaborador.

A partir de:

agendamentoDTO.IdColaborador

Buscar o colaborador.

Obter:

UsuarioId do colaborador.

Depois chamar:

BuscarUsuarioPorId

para obter o nome.

Variável:

@Nome

Nome do cliente:

Utilizar:

agendamentoDTO.IdUsuario

e chamar:

BuscarUsuarioPorId

Variável:

@NomeCliente

Serviço:

agendamentoDTO.DescricaoServico

Data:

agendamentoDTO.DataInicioAgendamento.ToString("dd/MM/yyyy")

Horário:

agendamentoDTO.DataInicioAgendamento.ToString("HH:mm")

Link:

UrlMakebe + "/Agendamentos"

Assunto:

Novo Agendamento

---------------------------------------------------------
PERSISTÊNCIA
---------------------------------------------------------

Inserir na tabela EMAIL.EmailEnvio:

DataCadastro = DateTime.Now
Processado = false
Tentativas = 0

DadosModelo:

serializar objeto contendo as variáveis do template.

DadosEnvio:

serializar objeto contendo:

{
  "Para":[
    {
      "Email":"email@destino.com",
      "Nome":"Nome"
    }
  ],
  "Copias":[],
  "Assunto":"Agendamento",
  "Sistema":"MakeBe",
  "Usuario":"Sistema"
}


E-mail Cliente:

Pasta:
agendamento

NomeArquivo:
 
email-agendamento-profissional.html 

E-mail Profissional:

Pasta:
agendamento

NomeArquivo:
email-agendamento.html

Campos comuns:

DataCadastro = DateTime.Now
Processado = false
Tentativas = 0
Erro = null
DataProcessamento = null