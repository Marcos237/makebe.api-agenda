Objetivo: Implementar uma funcionalidade completa baseada em MassTransit Request/Response para pesquisar itens da vitrine utilizando a view `vw_vitrine_servicos`.

Arquitetura obrigatória:

* Contracts
* DTOs
* Repository
* Service
* Consumer
* Dependency Injection
* MassTransit Request/Response

A implementação deve seguir o padrão já existente no projeto MakeBe.

---

# Entrada

O consumer receberá:

```csharp
public interface IPesquisarVitrineMessage
{
    string ValorItem { get; }
}
```

Implementação:

```csharp
[EntityName("pesquisar-vitrine")]
public class PesquisarVitrineMessage : IPesquisarVitrineMessage
{
    public string ValorItem { get; set; } = string.Empty;
}
```

---

# Consulta

Utilizar obrigatoriamente a view:

```sql
SELECT
    Id,
    Descricao,
    Valor,
    RazaoSocial,
    DescricaoCategoria,
    UrlImagem
FROM vw_vitrine_servicos
WHERE
(
    @ValorItem IS NULL
    OR @ValorItem = ''
    OR RazaoSocial LIKE CONCAT('%', @ValorItem, '%')
    OR Descricao LIKE CONCAT('%', @ValorItem, '%')
    OR DescricaoCategoria LIKE CONCAT('%', @ValorItem, '%')
    OR Nome LIKE CONCAT('%', @ValorItem, '%')
    OR Email LIKE CONCAT('%', @ValorItem, '%')
)
ORDER BY RazaoSocial;
```

Implementar utilizando Dapper.

---

# DTO de retorno

```csharp
public class ItemVitrineResponse
{
    public int Id { get; set; }

    public string Descricao { get; set; } = string.Empty;

    public decimal Valor { get; set; }

    public string RazaoSocial { get; set; } = string.Empty;

    public string DescricaoCategoria { get; set; } = string.Empty;

    public string UrlImagem { get; set; } = string.Empty;
}
```

---

# Contrato de resposta

```csharp
public interface IPesquisarVitrineResponse
{
    List<ItemVitrineResponse> Itens { get; }
}
```

Implementação:

```csharp
public class PesquisarVitrineResponse : IPesquisarVitrineResponse
{
    public List<ItemVitrineResponse> Itens { get; set; } = [];
}
```

---

# Repositório

Criar interface:

```csharp
public interface IVitrineRepository
{
    Task<List<ItemVitrineResponse>> PesquisarAsync(
        string valorItem,
        CancellationToken cancellationToken);
}
```

Implementação utilizando:

```csharp
IDbConnection
Dapper
vw_vitrine_servicos
```

---

# Serviço

Criar interface:

```csharp
public interface IVitrineService
{
    Task<List<ItemVitrineResponse>> PesquisarAsync(
        string valorItem,
        CancellationToken cancellationToken);
}
```

Implementação:

```csharp
public class VitrineService : IVitrineService
{
}
```

O serviço deve apenas orquestrar a chamada ao repositório.

---

# Consumer

Criar:

```csharp
public class PesquisarVitrineConsumer :
    IConsumer<PesquisarVitrineMessage>
{
}
```

Fluxo:

1. Receber o request.
2. Chamar o service.
3. Executar a pesquisa.
4. Responder utilizando MassTransit.

Exemplo:

```csharp
public async Task Consume(
    ConsumeContext<PesquisarVitrineMessage> context)
{
    var resultado = await _service.PesquisarAsync(
        context.Message.ValorItem,
        context.CancellationToken);

    await context.RespondAsync<IPesquisarVitrineResponse>(
        new
        {
            Itens = resultado
        });
}
```

Não utilizar:

```csharp
RespondAsync(bool)
RespondAsync(List<>)
```

Sempre responder com contrato.

---

# Registro do Consumer

Adicionar:

```csharp
cfg.AddConsumer<PesquisarVitrineConsumer>();
```

e

```csharp
endpointConfigurator.ConfigureConsumer<
    PesquisarVitrineConsumer>(context);
```

---

# Dependency Injection

Registrar:

```csharp
services.AddScoped<IVitrineRepository, VitrineRepository>();
services.AddScoped<IVitrineService, VitrineService>();
```

---

# Client Request

A chamada deverá funcionar da seguinte forma:

```csharp
var response =
    await _client.GetResponse<IPesquisarVitrineResponse>(
        new PesquisarVitrineMessage
        {
            ValorItem = "Eribela"
        });
```

---

# JSON esperado

```json
{
  "itens": [
    {
      "id": 1,
      "descricao": "Corte Masculino",
      "valor": 45.00,
      "razaoSocial": "Barbearia MakeBe",
      "descricaoCategoria": "Corte",
      "urlImagem": "https://site.com/imagens/corte.jpg"
    }
  ]
}
```

Gerar todos os arquivos completos, implementados e prontos para compilação seguindo o padrão atual utilizado no projeto MakeBe.
