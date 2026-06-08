# 🎟️ TicketSystem API

Uma API RESTful desenvolvida em **C# (.NET)** para o gerenciamento e a venda de ingressos para eventos. 

Este projeto foi construído focando nos princípios de **Clean Architecture** (Arquitetura Limpa) e **Domain-Driven Design (DDD)** para garantir que as regras de negócio estejam isoladas, seguras e fáceis de dar manutenção.

## 🚀 Tecnologias Utilizadas

* **Linguagem:** C# (.NET)
* **Acesso a Dados:** Entity Framework Core (EF Core)
* **Banco de Dados:** MySQL
* **Documentação da API:** Swagger / OpenAPI
* **Padrões de Projeto:** Repository Pattern, Use Cases, Value Objects, Separação de Entidades de Domínio vs. Modelos de Persistência.

## 🏗️ Arquitetura e Estrutura do Projeto

O sistema é dividido em Módulos de Negócio independentes (Contextos Delimitados), que se conectam apenas através do núcleo de infraestrutura compartilhado:

* 👤 **Users:** Gerenciamento de clientes. Possui proteção de domínio utilizando *Value Objects* para validação de E-mail e isolamento de regras de criação.
* 📅 **Events:** Gerenciamento do catálogo de eventos, controlando informações cruciais como Capacidade Total e Preço do ingresso.
* 💳 **Sales (Vendas):** O coração financeiro do sistema. Implementa uma **Máquina de Estados** robusta para o ciclo de vida do pedido (`TicketOrder`), garantindo transições seguras entre os seguintes status:
  * `Pending` (Pendente)
  * `Approved` (Aprovado)
  * `Rejected` (Rejeitado)
  * `Cancelled` (Cancelado)
* ⚙️ **Shared:** Módulo de infraestrutura que contém o `TicketContext` (banco de dados) e as classes de modelo "anêmicas" utilizadas exclusivamente para a persistência pelo EF Core, mantendo o Domínio principal 100% livre de dependências externas.

## 🛡️ Destaques das Regras de Negócio

* **Encapsulamento Blindado:** O status de uma venda não pode ser alterado diretamente de fora da Entidade. O fluxo exige a chamada de métodos específicos (ex: `ApprovePayment()`), que validam se a transação atual permite a mudança.
* **Prevenção de Dados Inválidos:** Uso de Value Objects, como o `record Email`, que utiliza validações internas garantindo que nenhum usuário seja criado com formato incorreto.
* **Consultas Otimizadas:** Delegação de processamento ao banco de dados (ex: `SumAsync()` no EF Core) para calcular o total de ingressos vendidos, garantindo alta performance sem sobrecarregar a memória da API.

---

## ⚙️ Como executar o projeto localmente

### 1. Pré-requisitos
* SDK do [.NET](https://dotnet.microsoft.com/download) instalado.
* Servidor MySQL rodando localmente (ou em nuvem).
* Visual Studio, VS Code ou Rider.

### 2. Clonar o repositório
Abra o seu terminal e rode o comando:
```bash
git clone [https://github.com/SEU_USUARIO/TicketSystem.git](https://github.com/SEU_USUARIO/TicketSystem.git)
cd TicketSystem
```
*(Não esqueça de trocar `SEU_USUARIO` pelo seu link real do GitHub)*

### 3. Configurar o Banco de Dados
Abra o arquivo `appsettings.json` (ou `appsettings.Development.json`) no projeto da API e configure a sua **Connection String** apontando para o seu banco MySQL local com seu usuário e senha.

### 4. Aplicar as Migrations do Entity Framework
No terminal, dentro da pasta principal da API, execute o comando abaixo para que o EF Core crie o banco de dados e todas as tabelas automaticamente:
```bash
dotnet ef database update
```

### 5. Rodar a Aplicação
Compile e inicie a API com o comando:
```bash
dotnet run
```

### 6. Testar no Swagger
Após a aplicação iniciar, abra o seu navegador e acesse a URL indicada no terminal adicionando `/swagger` no final. 
Exemplo: `http://localhost:5000/swagger` ou `https://localhost:5001/swagger`.

---

## 🗺️ Principais Endpoints da API

A API possui rotas semânticas e claras, divididas por domínios:

### Usuários (Users)
* `POST /api/User` - Cadastra um novo usuário.
* `GET /api/User/{id}` - Busca usuário por ID.
* `PUT /api/User/{id}` - Atualiza os dados do usuário.

### Eventos (Events)
* `POST /api/Event` - Cria um novo evento.
* `GET /api/Event/{id}` - Busca os detalhes de um evento.

### Vendas (Sales)
* `POST /api/Sale` - Cria uma nova intenção de compra (Status: Pendente).
* `GET /api/Sale/{id}` - Retorna os dados e o status atual de uma venda.
* `GET /api/Sale/event/{eventId}/total-sold` - Consulta de performance do total de ingressos já vendidos para um evento específico.
* `GET /api/Sale/user/{userId}` - Retorna o histórico de compras de um cliente.
* `PUT /api/Sale/{id}/approve` - Aprova o pagamento.
* `PUT /api/Sale/{id}/reject` - Rejeita a compra.
* `PUT /api/Sale/{id}/cancel` - Cancela um ingresso.
