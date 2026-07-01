# Controle de acesso por Permissão JWT - Colaborador

## Objetivo

Implementar uma camada reutilizável de autorização baseada na permissão presente no JWT para controlar a consulta de colaboradores.

A implementação deve seguir rigorosamente o padrão já existente no projeto:

- Repository
- Interface
- Domain Service
- Injeção de Dependência
- Entidades
- DTOs
- Dapper
- Convenções atuais da solução

Não criar soluções específicas apenas para este fluxo. O código deve ser reutilizável para futuras regras de autorização.

---

## JWT

O JWT possui os seguintes claims:

```json
{
  "UsuarioId": "341d0ac7-1732-489d-83ce-34640b5508fd",
  "permissao": "ffbfa665-0370-4953-8a33-3c1b1d87a091"
}
```

Precisamos obter:

```csharp
UsuarioId
PermissaoId
```

diretamente do JWT autenticado.

Não realizar parse manual do token.

Utilizar o mecanismo padrão já utilizado pelo projeto para leitura de Claims.

---

## Nova View

Existe a view:

```sql
vw_permissao_papeis
```

Estrutura:

```sql
SELECT
    Id,
    Descricao,
    PapeisId,
    Papeis
FROM vw_permissao_papeis
```

Consulta:

```sql
SELECT
    Id,
    Descricao,
    PapeisId,
    Papeis
FROM vw_permissao_papeis
WHERE Id = @PermissaoId
```

---

## Criar estrutura completa

Criar seguindo o padrão do projeto:

### Entidade

```csharp
PermissaoPapel
```

Mapear:

```csharp
Id
Descricao
PapeisId
Papeis
```

---

### Interface

```csharp
IPermissaoPapelRepository
```

---

### Repositório

```csharp
PermissaoPapelRepository
```

Responsável por consultar:

```sql
vw_permissao_papeis
```

---

### Registrar DI

Adicionar registro na injeção de dependência conforme padrão existente.

---

## Regra de Negócio

Alterar o método:

```csharp
ColaboradorDomainService.BuscarPaginadoPorConta(...)
```

---

### Passo 1

Obter:

```csharp
UsuarioId
PermissaoId
```

do usuário autenticado.

---

### Passo 2

Consultar:

```sql
vw_permissao_papeis
```

utilizando:

```csharp
PermissaoId
```

---

### Passo 3

Validar descrição da permissão.

Se:

```text
Administrador
```

ou

```text
Gestor
```

então manter fluxo atual.

Não alterar comportamento existente.

Deve continuar retornando todos os colaboradores da conta.

---

### Passo 4

Para qualquer outra permissão:

```text
Cliente
Colaborador
Sem Permissão
```

ou futuras permissões não privilegiadas

não permitir visualizar colaboradores da conta inteira.

Retornar somente o próprio colaborador.

Consulta:

```sql
SELECT
    Id,
    UsuarioId,
    Nome,
    Email,
    Cpf,
    Telefone,
    Instagran,
    CAST(PermissaoId AS CHAR) AS PermissaoId,
    MostrarVitrine,
    Status,
    UrlImagem,
    NomeImagem,
    DescricaoPermissao,
    ContaId
FROM vw_colaborador vc
WHERE vc.UsuarioId = @UsuarioId
AND vc.Status = 1
```

---

## Reutilização

Não deixar a regra presa ao ColaboradorDomainService.

Criar componente reutilizável para futuras validações de permissão.

Exemplo:

```csharp
IPermissaoUsuarioService
```

ou

```csharp
IUsuarioPermissaoService
```

conforme padrão do projeto.

Objetivo:

Permitir reutilização futura em outros Domain Services.

---

## Requisitos

- Seguir padrão atual do projeto.
- Utilizar Dapper.
- Não duplicar código.
- Não realizar consultas diretas dentro do Domain Service.
- Toda consulta deve passar por Repository.
- Registrar tudo na DI.
- Não quebrar funcionalidades existentes.
- Manter compatibilidade com filtros e paginação atuais.
- Entregar código pronto para compilação.

---

## Entregáveis

Ao finalizar apresentar:

- Arquivos criados.
- Arquivos alterados.
- Interfaces criadas.
- Repositórios criados.
- Registros adicionados na DI.
- Alterações realizadas no ColaboradorDomainService.
- Resumo da regra implementada.