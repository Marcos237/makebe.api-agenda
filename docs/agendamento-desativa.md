# Skill: Desativar Agendamento por Mensageria

## Objetivo

Implementar uma Consumer responsável por receber uma mensagem contendo o `Id` do agendamento e realizar sua desativação através do repositório.

## Requisitos

### Mensagem

Criar uma mensagem de entrada contendo:

```csharp
public class DesativarAgendamentoMessage
{
    public int Id { get; set; }
}
```

### Consumer

Criar uma Consumer que:

1. Receba a mensagem `DesativarAgendamentoMessage`.
2. Valide se o `Id` é maior que zero.
3. Chame o método `Desativar(int id)` do repositório.
4. Registre logs de sucesso e erro.
5. Retorne o resultado da operação.

Exemplo:

```csharp
public class DesativarAgendamentoConsumer : IConsumer<DesativarAgendamentoMessage>
{
    private readonly IAgendamentoRepository _agendamentoRepository;
    private readonly ILogger<DesativarAgendamentoConsumer> _logger;

    public DesativarAgendamentoConsumer(
        IAgendamentoRepository agendamentoRepository,
        ILogger<DesativarAgendamentoConsumer> logger)
    {
        _agendamentoRepository = agendamentoRepository;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<DesativarAgendamentoMessage> context)
    {
        var id = context.Message.Id;

        if (id <= 0)
        {
            _logger.LogWarning("Id de agendamento inválido: {Id}", id);
            return;
        }

        var desativado = await _agendamentoRepository.Desativar(id);

        if (desativado)
        {
            _logger.LogInformation("Agendamento {Id} desativado com sucesso.", id);
        }
        else
        {
            _logger.LogWarning("Nenhum agendamento encontrado para desativar. Id: {Id}", id);
        }
    }
}
```

### Repositório

Utilizar obrigatoriamente o método existente:

```csharp
public async Task<bool> Desativar(int id)
{
    var sql = @"UPDATE Agendamento
                SET Ativo = 0
                WHERE Id = @Id";

    var response = await _dbAgenda.Connection.ExecuteAsync(sql, new
    {
        Id = id
    });

    return response > 0;
}
```

## Critérios de Aceitação

* Consumer criada e registrada no container de DI.
* Mensagem criada.
* Consumer recebe o Id do agendamento.
* Método `Desativar(int id)` é chamado.
* Retorna `true` quando pelo menos um registro for atualizado.
* Retorna `false` quando nenhum registro for atualizado.
* Logs de sucesso e falha implementados.
* Código segue os padrões existentes do projeto.
