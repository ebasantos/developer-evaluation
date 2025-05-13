# Ambev Developer Evaluation

Este projeto é uma avaliação de desenvolvedor para a Ambev, implementando um sistema de vendas com regras de negócio específicas.

## Requisitos

- .NET 8.0 SDK
- Docker e Docker Compose
- PostgreSQL 13
- RabbitMQ 3-management

## Configuração do Ambiente

1. Clone o repositório
2. Navegue até a pasta do projeto:
   ```bash
   cd template/backend
   ```

3. Configure as variáveis de ambiente:
   - O banco de dados PostgreSQL será configurado automaticamente com:
     - Usuário: postgres
     - Senha: ev@luAt10n
     - Banco: developer_evaluation

4. Execute o projeto usando Docker Compose:
   ```bash
   docker-compose up -d
   ```

## Estrutura do Projeto

O projeto está organizado nas seguintes camadas:

- **Domain**: Contém as entidades e regras de negócio
  - `Sale`: Entidade principal de venda
  - `SaleItem`: Itens de venda com regras de desconto
  - `User`: Entidade de usuário

- **Application**: Contém a lógica de aplicação
  - Commands e Handlers para operações de venda
  - Validadores de requisições
  - Mapeamentos DTO

- **ORM**: Configuração do Entity Framework Core
  - Mapeamentos das entidades
  - Configuração do banco de dados
  - Repositórios

- **WebApi**: API REST
  - Controllers
  - Middlewares
  - Configuração de autenticação

## Regras de Negócio

### Vendas

1. **Limite de Quantidade**:
   - Não é permitido vender mais de 20 itens iguais

2. **Descontos por Quantidade**:
   - 20% de desconto para quantidades entre 10 e 20
   - 10% de desconto para quantidades entre 5 e 9
   - Sem desconto para quantidades abaixo de 5

3. **Cancelamento**:
   - Vendas podem ser canceladas
   - Itens individuais podem ser cancelados
   - Eventos de domínio são disparados para cancelamentos

## Endpoints da API

### Vendas

- `POST /api/sales`: Criar nova venda
- `PUT /api/sales/{id}`: Atualizar venda
- `DELETE /api/sales/{id}`: Cancelar venda
- `DELETE /api/sales/{id}/items/{itemId}`: Cancelar item de venda

### Autenticação

- `POST /api/auth/login`: Login de usuário
- `POST /api/auth/register`: Registro de novo usuário

## Testes

O projeto inclui testes unitários para:

- Entidades de domínio
- Handlers de comando
- Validadores de requisição

Para executar os testes:
```bash
dotnet test
```

## Migrações

Para criar uma nova migração:
```bash
cd src/Ambev.DeveloperEvaluation.ORM
dotnet ef migrations add NomeDaMigracao
```

Para aplicar as migrações:
```bash
dotnet ef database update
```

## Observações

- O projeto usa PostgreSQL como banco de dados principal
- RabbitMQ é usado para mensageria
- A autenticação é feita via JWT

---
