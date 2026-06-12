# Skill para retornar períodos disponíveis para agendamento

## Objetivo

Criar um Consumer responsável por retornar os períodos que podem ser exibidos na tela de agendamento.

O retorno deve ser uma lista de:

```csharp
public class PeriodoDTO
{
    public DateTime Inicio { get; set; }
    public DateTime Fim { get; set; }
    public bool IsAgendado { get; set; }
}
```

Seguir rigorosamente os padrões já existentes do projeto (Consumer, Application, Domain Service, Repository, DTOs, Validators, etc.) sem alterar regras existentes.

---

## Primeiro Passo - Buscar Serviço

O Consumer receberá:

```csharp
int IdServico
```

Utilizar o método já existente no repositório:

```csharp
public async Task<Servico> BuscarPorId(int id)
{
    var sql = @"SELECT * from Servicos s
                WHERE s.Status = 1
                AND s.Id = @Id";

    var response = await _dbAgenda.Connection
        .QueryFirstOrDefaultAsync<Servico>(sql, new { Id = id })
        ?? new Servico();

    return response;
}
```

A propriedade utilizada será:

```csharp
Servico.Periodo
```

Esse valor representa a duração de cada slot em minutos.

Exemplo:

```text
Periodo = 30
```

significa:

```text
09:00
09:30
10:00
10:30
...
```

---

## Segundo Passo - Buscar agenda do colaborador

Criar consulta no repositório:

```csharp
AgendamentoColaboradorRepository
```

Criar método específico para retornar:

```sql
SELECT
    ac.Id AS IdAgendaColaborador,
    cp.ColaboradorId,
    cp.PeriodoInativoInicio,
    cp.PeriodoInativoFim,
    a.AgendaBloqueadaInicio,
    a.AgendaBloqueadaFim,
    ag.DataInicioAgendamento,
    ag.DataTerminoAgendamento
FROM ColaboradorProfissional cp
INNER JOIN AgendaColaborador ac
    ON ac.IdColaborador = cp.ColaboradorId
INNER JOIN Agenda a
    ON a.Id = ac.IdAgenda
LEFT JOIN Agendamento ag
    ON ag.IdAgendaColaborador = ac.Id
WHERE ac.Status = 1
AND ac.IdColaborador = @IdColaborador
```

Criar DTO específico para retorno desta consulta.

---

## Terceiro Passo - Construção da grade diária

Considerar inicialmente um período completo de 24 horas:

```text
00:00 até 23:59
```

A partir desse período remover horários indisponíveis.

### Regra 1 - Período Inativo

Utilizar apenas a parte da hora.

Exemplo:

```text
PeriodoInativoInicio = 20:00
PeriodoInativoFim = 08:00
```

Significa que o colaborador NÃO pode atender:

```text
20:00 até 08:00
```

Logo devem permanecer apenas:

```text
08:01 até 19:59
```

ou equivalente conforme a granularidade do período.

---

### Regra 2 - Agenda Bloqueada

Aplicar a mesma lógica.

Exemplo:

```text
AgendaBloqueadaInicio = 12:00
AgendaBloqueadaFim = 13:00
```

Remover todos os horários compreendidos nesse intervalo.

O resultado final deverá conter apenas horários realmente disponíveis.

---

## Quarto Passo - Gerar os slots

Após obter os períodos válidos:

Dividir os intervalos restantes utilizando:

```csharp
Servico.Periodo
```

Exemplo:

```text
Período disponível:
09:00 às 12:00

Periodo:
30 minutos
```

Gerar:

```text
09:00
09:30
10:00
10:30
11:00
11:30
```

Criando:

```csharp
new PeriodoDTO
{
    Inicio = slotInicio,
    Fim = slotFim,
    IsAgendado = false
}
```

---

## Quinto Passo - Marcar períodos agendados

Se existir:

```csharp
DataInicioAgendamento
```

e

```csharp
DataTerminoAgendamento
```

localizar o slot correspondente.

Exemplo:

```text
Slot:
10:00 às 10:30

Agendamento:
10:00 às 10:30
```

Então:

```csharp
IsAgendado = true
```

Caso não exista agendamento para o slot:

```csharp
IsAgendado = false
```

---

## Regras Importantes

* Não alterar regras existentes.
* Não alterar contratos existentes.
* Não alterar Consumers já existentes.
* Criar novas classes seguindo o padrão atual do projeto.
* Colocar toda a lógica de negócio na camada Domain.
* Repository apenas consulta dados.
* Consumer apenas orquestra a chamada.
* Utilizar async/await.
* Utilizar DTOs específicos.
* Utilizar injeção de dependência seguindo o padrão atual.
* Criar testes unitários para a regra de geração dos períodos.
* Garantir que períodos sobrepostos sejam tratados corretamente.
* Garantir que intervalos bloqueados sejam removidos antes da geração dos slots.
* Garantir que o período seja respeitado exatamente em minutos.
