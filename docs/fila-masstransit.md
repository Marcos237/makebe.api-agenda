# Arquitetura de Mensageria - MassTransit

## Objetivo

Este projeto utiliza MassTransit com RabbitMQ apenas para publicação de eventos.

Os consumers NÃO pertencem a esta aplicação.

Esta aplicação funciona como:

- Producer
- Publisher
- Dispatcher de eventos

O processamento ocorre em microsserviços/aplicações externas.

---

# Estrutura Atual do Projeto

api.makebe.agenda
api.makebe.agenda.applications
api.makebe.agenda.domain
api.makebe.agenda.infra.crosscutting
api.makebe.agenda.infra.crosscutting.data
api.makebe.agenda.infra.crosscutting.ioc

---

# Responsabilidade das Camadas

## Domain

Responsável apenas por:

- Entidades
- Regras de negócio
- Interfaces
- Objetos de domínio

NUNCA colocar:

- MassTransit
- RabbitMQ
- Consumer
- Código de mensageria

---

## Applications

Responsável por:

- Casos de uso
- Services
- DTOs
- Commands
- Orquestração

Pode disparar eventos através de interfaces.

Nunca acessar RabbitMQ diretamente.

---

## Infra.CrossCutting

Responsável por:

- Publicação de eventos
- Integração MassTransit
- Contratos de eventos
- Configuração de filas
- BusEvent
- Interfaces de mensageria

Estrutura esperada:

Infra.CrossCutting/
 └── Events/
      ├── Interfaces/
      ├── Usuarios/
      ├── Permissoes/
      ├── Contas/
      └── UsuarioEvents/

---

# Regras de Mensageria

Nunca criar:

- IConsumer<T>
- ReceiveEndpoint
- ConsumerDefinition

Os consumers pertencem a outro projeto/microsserviço.

---

# Publicação de Eventos

Toda publicação deve ocorrer através de:

```csharp id="hy7i3n"
IBusEvent