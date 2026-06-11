# Implementar Consumer de Consulta de Meus Agendamentos

## Objetivo

Implementar um novo consumer responsável por retornar os agendamentos do usuário logado.

A implementação deve seguir rigorosamente os padrões já existentes no projeto para consumers, eventos, handlers, DTOs, mapeamentos, injeção de dependência, nomenclatura e organização de arquivos.

Não alterar regras existentes do sistema.

## Regra de consulta

A consulta deverá retornar os seguintes dados:

```sql
SELECT
    a.IdUsuario,
    a.DataInicioAgendamento,
    a.DataTerminoAgendamento,
    s.Descricao,
    ac.IdColaborador,
    c.UsuarioId AS IdColaboradorUsuario
FROM Agendamento a
INNER JOIN AgendaColaborador ac ON ac.Id = a.IdAgendaColaborador
INNER JOIN Servicos s ON s.Id = a.IdServico
INNER JOIN Colaborador c ON c.Id = ac.IdColaborador
WHERE a.IdUsuario = @IdUsuario
```

O filtro deve utilizar o usuário logado recebido pelo fluxo atual da aplicação.

Não utilizar valores fixos.

## Dados retornados

O retorno deve conter:

* IdUsuario
* DataInicioAgendamento
* DataTerminoAgendamento
* DescricaoServico
* IdColaborador
* IdColaboradorUsuario
* NomeColaborador
* EhDesativado

## Consulta do nome do colaborador

Após obter os agendamentos, utilizar o consumer já existente:

```text
UsuarioConsultadoPorIdEvent
```

para buscar os dados do colaborador.

Utilizar o campo:

```text
IdColaboradorUsuario
```

como identificador da consulta.

Extrair o nome retornado pelo consumer e preencher:

```csharp
NomeColaborador
```

Seguir exatamente o mesmo padrão utilizado pelos demais consumers do projeto para chamadas entre consumers.

Não criar implementações paralelas.

Não duplicar lógica já existente.

## Regra de negócio

Criar uma propriedade:

```csharp
bool EhDesativado
```

A regra deve ser:

```csharp
EhDesativado = DataInicioAgendamento > DateTime.Now;
```

Ou seja:

* Agendamentos futuros → EhDesativado = true
* Agendamentos passados ou atuais → EhDesativado = false

## Método de extensão

Criar um método de extensão dentro da camada de domínio seguindo o padrão já existente no projeto.

Exemplo esperado:

```csharp
public static bool CalcularEhDesativado(this AgendamentoConsultaDTO agendamento)
{
    return agendamento.DataInicioAgendamento > DateTime.Now;
}
```

O nome exato deve seguir a convenção utilizada no domínio.

Evitar colocar regra de negócio diretamente no consumer.

A regra deve ficar centralizada no método de extensão.

## Consumer

Implementar:

* Event de consulta
* Consumer
* DTOs necessários
* Mapeamentos necessários

Seguindo exatamente o padrão dos demais consumers já existentes.

## Performance

Caso existam múltiplos agendamentos do mesmo colaborador:

* Evitar chamadas desnecessárias ao consumer UsuarioConsultadoPorIdEvent.
* Reutilizar informações já obtidas quando possível.
* Seguir o padrão já utilizado no projeto para evitar processamento duplicado.

## Critérios de aceite

* Consumer criado seguindo o padrão atual do projeto.
* Consulta filtrando pelo usuário logado.
* Serviço retornado corretamente.
* Nome do colaborador obtido através do consumer UsuarioConsultadoPorIdEvent.
* Propriedade EhDesativado preenchida corretamente.
* Regra implementada através de método de extensão no domínio.
* Nenhuma regra existente alterada.
* Nenhum comportamento existente impactado.
* Código compatível com os padrões de arquitetura já adotados no projeto.
