# Alteração do Cadastro de Colaborador Profissional — Múltiplos Serviços

## Objetivo

Alterar o cadastro de `ColaboradorProfissional` para permitir que um colaborador possua uma lista de serviços, em vez de apenas um único serviço.

O colaborador poderá possuir **no máximo 10 serviços** associados.

A tabela de relacionamento existente no MySQL será utilizada:

```sql
SELECT
    cs.Id,
    cs.IdColaborador,
    cs.IdServico,
    cs.DataCadastro
FROM ColaboradorServicos cs;
```

Estrutura esperada:

```text
ColaboradorServicos
--------------------
Id
IdColaborador
IdServico
DataCadastro
```

## Model

O cadastro deverá utilizar a propriedade:

```csharp
public IEnumerable<ColaboradorServicos>? Servicos { get; set; }
```

Cada item da lista deverá representar um serviço associado ao colaborador.

Exemplo:

```csharp
public class ColaboradorServicos
{
    public int Id { get; set; }
    public int IdColaborador { get; set; }
    public int IdServico { get; set; }
    public DateTime DataCadastro { get; set; }
}
```

## Regra de negócio

Um `ColaboradorProfissional` poderá possuir:

* Nenhum serviço;
* Um serviço;
* Vários serviços;
* No máximo **10 serviços**.

Não deverá ser permitido cadastrar o mesmo `IdServico` mais de uma vez para o mesmo colaborador.

A validação do limite de 10 serviços deverá existir também no backend, não apenas na tela.

## Consulta do colaborador

Ao buscar um colaborador para edição, também deverá ser carregada a lista de serviços cadastrados na tabela `ColaboradorServicos`.

Exemplo:

```sql
SELECT
    Id,
    IdColaborador,
    IdServico,
    DataCadastro
FROM ColaboradorServicos
WHERE IdColaborador = @IdColaborador;
```

O resultado deverá preencher:

```csharp
colaborador.Servicos
```

A API deverá retornar os serviços associados para que a tela consiga exibir os itens atualmente selecionados.

## Tela de cadastro/edição

O campo atual de serviço único deverá ser substituído por uma seleção que permita múltiplos serviços.

A tela deverá:

* Exibir os serviços disponíveis;
* Permitir selecionar até 10 serviços;
* Ao editar um colaborador, carregar os serviços já cadastrados;
* Permitir adicionar novos serviços;
* Permitir remover serviços selecionados;
* Não permitir serviços duplicados;
* Informar ao usuário quando o limite de 10 serviços for atingido.

## Inclusão

Ao cadastrar um novo colaborador, após obter o `IdColaborador`, deverão ser inseridos os serviços selecionados.

Exemplo:

```sql
INSERT INTO ColaboradorServicos
(
    IdColaborador,
    IdServico,
    DataCadastro
)
VALUES
(
    @IdColaborador,
    @IdServico,
    NOW()
);
```

A operação deverá ser realizada para cada serviço recebido na lista:

```csharp
Servicos
```

## Atualização

Na edição do colaborador, a lista recebida pela API deverá ser comparada com os serviços atualmente cadastrados no banco.

Exemplo:

Banco:

```text
1
2
3
```

Nova lista recebida:

```text
2
3
4
```

Resultado esperado:

```text
Serviço 1 -> remover
Serviço 2 -> manter
Serviço 3 -> manter
Serviço 4 -> inserir
```

Não é necessário remover e inserir novamente os serviços que continuam associados ao colaborador.

### Serviços para adicionar

Identificar os serviços presentes na nova lista que ainda não existem no banco.

Conceitualmente:

```csharp
var adicionar = novosServicos
    .Where(id => !servicosAtuais.Contains(id));
```

Para cada serviço novo:

```sql
INSERT INTO ColaboradorServicos
(
    IdColaborador,
    IdServico,
    DataCadastro
)
VALUES
(
    @IdColaborador,
    @IdServico,
    NOW()
);
```

### Serviços para remover

Identificar os serviços existentes no banco que não estão mais presentes na lista enviada pela tela.

Conceitualmente:

```csharp
var remover = servicosAtuais
    .Where(id => !novosServicos.Contains(id));
```

Remover somente esses relacionamentos:

```sql
DELETE FROM ColaboradorServicos
WHERE IdColaborador = @IdColaborador
AND IdServico = @IdServico;
```

## Lista vazia

Caso a tela envie uma lista vazia, todos os serviços atualmente associados ao colaborador deverão ser removidos.

```sql
DELETE FROM ColaboradorServicos
WHERE IdColaborador = @IdColaborador;
```

## Validações no backend

Antes de salvar:

```csharp
if (request.Servicos?.Count() > 10)
{
    throw new Exception(
        "O colaborador pode possuir no máximo 10 serviços."
    );
}
```

Também deverá ser validada a existência de serviços duplicados.

Exemplo:

```csharp
var servicos = request.Servicos?.ToList()
    ?? new List<ColaboradorServicos>();

if (servicos
    .GroupBy(x => x.IdServico)
    .Any(x => x.Count() > 1))
{
    throw new Exception(
        "Não é permitido adicionar o mesmo serviço mais de uma vez."
    );
}
```

## Integridade no banco

Para impedir que o mesmo serviço seja associado duas vezes ao mesmo colaborador, criar uma restrição única para:

```text
IdColaborador + IdServico
```

Exemplo:

```sql
ALTER TABLE ColaboradorServicos
ADD CONSTRAINT UK_ColaboradorServicos_Colaborador_Servico
UNIQUE (IdColaborador, IdServico);
```

## Fluxo esperado

### Cadastro

```text
Tela
  ↓
Seleciona até 10 serviços
  ↓
API recebe Servicos[]
  ↓
Salva ColaboradorProfissional
  ↓
Obtém IdColaborador
  ↓
Insere registros em ColaboradorServicos
```

### Edição

```text
Busca ColaboradorProfissional
  ↓
Busca ColaboradorServicos
  ↓
API retorna Servicos[]
  ↓
Tela apresenta serviços selecionados
  ↓
Usuário adiciona/remove serviços
  ↓
API recebe nova lista
  ↓
Compara banco x nova lista
  ↓
Insere novos
  ↓
Remove os que saíram
  ↓
Mantém os que não mudaram
```

## Critérios de aceite

1. O cadastro deve permitir selecionar até 10 serviços.
2. O colaborador pode possuir mais de um serviço.
3. Ao abrir a edição, os serviços cadastrados devem aparecer selecionados.
4. Deve ser possível adicionar um novo serviço.
5. Deve ser possível remover um serviço existente.
6. Serviços que permaneceram selecionados não devem ser recriados.
7. Não deve ser possível cadastrar serviços duplicados.
8. O backend deve impedir listas com mais de 10 serviços.
9. Uma lista vazia deve remover os serviços associados ao colaborador.
10. As associações devem ser persistidas na tabela `ColaboradorServicos`.
11. A API deve retornar a lista `Servicos` ao consultar o colaborador.
12. Cadastro/edição do colaborador e atualização dos serviços devem ocorrer de forma transacional, evitando salvar apenas parte das alterações em caso de erro.
