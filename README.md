# Gestão de Clientes API

API para gestão de clientes desenvolvida em .NET 9.0, adotando **Domain-Driven Design (DDD)**, **Clean Architecture** e seguindo rigorosamente os princípios **SOLID**. O projeto utiliza **NHibernate** como ORM e é totalmente containerizado com Docker.

## 🚀 Tecnologias e Práticas

- **.NET 9.0** (ASP.NET Core Web API)
- **Domain-Driven Design (DDD)** (Modelagem rica do domínio)
- **SOLID Principles** (Boas práticas de design de software)
- **Clean Architecture** (Separação de responsabilidades em camadas)
- **NHibernate** (ORM robusto para persistência)
- **SQLite** (Banco de dados leve e portátil)
- **MediatR** (Implementação do padrão Mediator para CQRS)
- **CQRS** (Command Query Responsibility Segregation)
- **Docker & Docker Compose** (Ambiente padronizado)
- **Swagger / OpenAPI** (Documentação viva)

## 📋 Pré-requisitos

Para rodar o projeto, você precisa apenas de:

- [Docker Desktop](https://www.docker.com/products/docker-desktop/) instalado e em execução.

> **Nota:** Não é necessário ter o .NET SDK instalado localmente para rodar a aplicação, pois todo o build e runtime ocorrem dentro do Docker.

## 🛠️ Como Rodar o Projeto

1. Abra o terminal na pasta raiz do projeto.
2. Execute o comando para construir e subir os containers:

```bash
docker-compose up -d --build
```

O comando irá:
1. Restaurar as dependências e compilar o projeto (Build Stage).
2. Gerar uma imagem Docker otimizada.
3. Iniciar o container da API mapeando a porta `8080`.
4. Criar/Mapear o volume para o banco de dados SQLite persistir os dados.
5. Executar automaticamente as atualizações de schema do banco de dados (via NHibernate SchemaUpdate).

## 🔌 Acessando a API

Após o comando finalizar, a API estará disponível em:

- **Swagger UI (Documentação Interativa)**: [http://localhost:8080/swagger](http://localhost:8080/swagger)
- **Especificação OpenAPI (JSON)**: [http://localhost:8080/openapi/v1.json](http://localhost:8080/openapi/v1.json)

### Exemplo de Uso

Para criar um novo cliente, você pode usar o Swagger ou fazer uma requisição HTTP direta.

**Endpoint**: `POST /api/Clientes`

**Exemplo de Payload (JSON)**:
```json
{
  "nome": "Empresa Exemplo S.A.",
  "email": "contato@exemplo.com.br",
  "cnpj": "11.222.333/0001-81"
}
```

**Exemplo com cURL (Terminal/Insomnia)**:
```bash
curl --request POST \
  --url http://localhost:8080/api/Clientes \
  --header 'Content-Type: application/json' \
  --data '{
    "nome": "Empresa Exemplo S.A.",
    "email": "contato@exemplo.com.br",
    "cnpj": "11.222.333/0001-81"
}'
```

> **Atenção:** O sistema possui validação real de CNPJ encapsulada em um **Value Object**. Utilize um CNPJ válido (matematicamente correto) para testar, caso contrário receberá um erro `400 Bad Request`.

## 🏗️ Estrutura e Arquitetura (DDD & Clean Arch)

O projeto está organizado para respeitar a **Dependency Rule** da Clean Architecture e aplicar os conceitos táticos do **DDD**:

1.  **GestaoClientes.Domain** (Core):
    *   Camada mais interna, sem dependências externas.
    *   **Entidades:** Classes com identidade definida (ex: `Cliente`).
    *   **Value Objects:** Objetos imutáveis definidos por seus atributos (ex: `Cnpj`, `Email`), garantindo a integridade dos dados e encapsulando regras de validação.
    *   **Interfaces de Repositório:** Definição dos contratos para persistência (Inversão de Dependência - DIP).

2.  **GestaoClientes.Application**:
    *   Orquestra os fluxos de negócio.
    *   **Use Cases (Features):** Implementados via **Commands** (Escrita) e **Queries** (Leitura) com **CQRS**.
    *   **DTOs:** Objetos de transferência de dados para desacoplar o domínio da camada de apresentação.

3.  **GestaoClientes.Infrastructure**:
    *   Camada de detalhes técnicos.
    *   **Persistência:** Implementação dos Repositórios usando **NHibernate**.
    *   **Mapeamento:** Configuração ORM via código (`ClassMapping`).
    *   **UnitOfWork:** Gestão de transações atômicas.

4.  **GestaoClientes.API**:
    *   Camada de Apresentação.
    *   **Controllers:** Pontos de entrada RESTful simples.
    *   **Middlewares:** Tratamento global de exceções.
    *   **Injeção de Dependência:** Configuração do container de serviços.

## 🛡️ Qualidade e Boas Práticas

*   **SOLID:**
    *   **SRP (Single Responsibility Principle):** Cada classe tem uma única responsabilidade (ex: Handlers focados em um único caso de uso).
    *   **OCP (Open/Closed Principle):** Arquitetura extensível via interfaces e mediadores.
    *   **LSP (Liskov Substitution Principle):** Implementações de repositório substituíveis.
    *   **ISP (Interface Segregation Principle):** Interfaces focadas (ex: `IUnitOfWork`, `IRepository`).
    *   **DIP (Dependency Inversion Principle):** Camadas de alto nível não dependem de detalhes de implementação.
*   **Tratamento de Erros:** Exceções de domínio (`DomainException`) são tratadas elegantemente, retornando feedbacks claros ao cliente (HTTP 400), enquanto erros inesperados são mascarados (HTTP 500).

---
Desenvolvido em 2025.