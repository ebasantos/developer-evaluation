# 🚀 Avaliação Técnica - Sistema de Vendas


## 📋 Descrição do Projeto

Este projeto consiste em uma API completa para controle de vendas, incluindo:
- Cadastro, atualização, consulta e cancelamento de vendas
- Controle de clientes,  itens vendidos
- Aplicação de descontos automáticos conforme a quantidade de itens
- Publicação de eventos de domínio (ex: venda criada, modificada, cancelada)
- Integração com PostgreSQL e RabbitMQ

## 🛠️ Tecnologias Utilizadas

- .NET 8
- Entity Framework Core
- PostgreSQL
- RabbitMQ (MassTransit)
- Docker & Docker Compose
- xUnit, Moq (testes)

## ⚙️ Como Executar o Projeto

### Pré-requisitos
- Docker e Docker Compose instalados
- .NET 8 SDK instalado (opcional, para rodar comandos locais)

### Passos para rodar tudo com Docker

```bash
git clone <url-do-seu-repositorio>
cd abi-gth-omnia-developer-evaluation/template/backend
cp ../.env.example .env # se existir arquivo de exemplo
# Suba os containers
sudo docker-compose up --build
```

A API estará disponível em: [http://localhost:5000](http://localhost:5000)

### Aplicar Migrações Manualmente (opcional)
Se quiser rodar as migrações manualmente:
```bash
cd src/Ambev.DeveloperEvaluation.ORM
dotnet ef database update --connection "Host=localhost;Database=developer_evaluation;Username=developer;Password=ev@luAt10n"
```

## 🧪 Rodando os Testes

```bash
cd tests/Ambev.DeveloperEvaluation.Unit
dotnet test
```

## 📦 Estrutura do Projeto

- `src/` - Código-fonte da aplicação
- `tests/` - Testes automatizados
- `docker-compose.yml` - Orquestração dos serviços

## 📚 Documentação

- [Visão Geral](.doc/overview.md)
- [Stack Tecnológico](.doc/tech-stack.md)
- [Regras de Negócio](README.md#descrição-do-projeto)

## 👤 Autor

- Desenvolvido por Erik Santos

---
