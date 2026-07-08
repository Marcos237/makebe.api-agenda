# Skill: categoria-servico

## Objetivo

Implementar a associação de categorias aos serviços.

Ao cadastrar ou editar um serviço, o front-end deverá exibir um Select carregado pela consulta:

```sql
SELECT *
FROM CategoriaItem
WHERE Status = 1
ORDER BY Descricao;
```

A categoria selecionada deverá ser enviada junto com o objeto `ServicoDTO` e persistida após a gravação do serviço.



### 2. DTO

Adicionar a propriedade:

```csharp
public int CategoriaItemId { get; set; }
```

na classe:

```csharp
ServicoDTO
```

---

### 3. Entidade Categoria

Criar a entidade:

```csharp
public class Categoria
{
    public int Id { get; set; }

    public int ServicoId { get; set; }

    public string Descricao { get; set; } = string.Empty;

    public DateTime DataCadastro { get; set; }

    public bool Ativo { get; set; }
}
```

---

### 4. Tabela Categoria

Utilizar a tabela existente:

```sql
CREATE TABLE Categoria
(
    Id INT AUTO_INCREMENT PRIMARY KEY,
    ServicoId INT NOT NULL,
    Descricao VARCHAR(200) NOT NULL,
    DataCadastro DATETIME NOT NULL,
    Ativo BIT NOT NULL
);
```

---

### 5. Repositório

Criar:

```csharp
ICategoriaRepository
CategoriaRepository
```

com os métodos:

```csharp
Task<int> Salvar(Categoria categoria);

Task<IEnumerable<Categoria>> BuscarPorServico(int servicoId);
```

---

### 6. Domain Service

Criar:

```csharp
ICategoriaDomainService
CategoriaDomainService
```

Implementando:

```csharp
Task<int> Salvar(Categoria categoria);

Task<IEnumerable<Categoria>> BuscarPorServico(int servicoId);
```

---

### 7. Dependency Injection

Registrar:

```csharp
services.AddScoped<ICategoriaRepository, CategoriaRepository>();

services.AddScoped<ICategoriaDomainService, CategoriaDomainService>();
```

---

### 8. Persistência do Serviço

Após executar:

```csharp
var servicoRetorno = await _servicosDomainService.Persitir(servicoMap);
```

buscar a categoria selecionada em:

```sql
CategoriaItem
```

utilizando:

```csharp
item.CategoriaItemId
```

e criar um registro na tabela:

```sql
Categoria
```

preenchendo:

| Campo        | Valor                   |
| ------------ | ----------------------- |
| ServicoId    | servicoRetorno          |
| Descricao    | CategoriaItem.Descricao |
| DataCadastro | DateTime.Now            |
| Status       | true                    |

Exemplo:

```csharp
var categoriaItem =
    await _categoriaItemDomainService.BuscarPorId(item.CategoriaItemId);

if (categoriaItem != null)
{
    await _categoriaDomainService.Salvar(new Categoria
    {
        ServicoId = servicoRetorno,
        Descricao = categoriaItem.Descricao,
        DataCadastro = DateTime.Now,
        Status = true
    });
}
```

---

### 9. Transação

A gravação da categoria deve ocorrer dentro da mesma transação já utilizada em:

```csharp
ServicosApplicationService.Persitir
```

Se ocorrer erro em qualquer etapa:

* Serviço não deve ser salvo.
* Categoria não deve ser salva.
* Executar Rollback.

---

### 10. Padrão do Projeto

Seguir exatamente o padrão já existente no projeto para:

* Repository
* Domain Service
* Application Service
* Dependency Injection
* Unit Of Work
* Dapper
* Entidades
* DTOs

Não criar padrões diferentes dos já utilizados no sistema.
