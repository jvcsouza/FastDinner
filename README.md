# 🍽️ Sistema de Gerenciamento de Restaurante - FastDinner 🍔

Bem-vindo ao Sistema de Gerenciamento de Restaurante, onde a comida é boa e a gestão é ainda melhor! 🎉

![Chef Cozinheiro](https://media.giphy.com/media/13LlAxmDwAkopO/giphy.gif)

## 🏗️ Arquitetura

O FastDinner implementa a **Clean Architecture (Arquitetura Limpa)** com **Domain-Driven Design (DDD)**, seguindo os princípios SOLID e padrões modernos de desenvolvimento. A aplicação está organizada em camadas bem definidas, cada uma com responsabilidades específicas.

### 📚 Camadas da Aplicação

```
FastDinner/
├── 🚀 FastDinner.Api/           # Camada de apresentação (Controllers, Middleware)
├── 🧠 FastDinner.Application/   # Camada de aplicação (CQRS, Handlers)
├── 🏛️ FastDinner.Domain/        # Camada de domínio (Entidades, Regras de negócio)
├── 🔧 FastDinner.Infrastructure/# Camada de infraestrutura (Repositórios, Serviços)
└── 📋 FastDinner.Contracts/     # Camada de contratos (DTOs, Interfaces)
```

### 🔄 Fluxo de Dados

```
HTTP Request → API → Application → Domain ← Infrastructure
     ↑                                    ↓
Contracts ← Response ← Handlers ← Repositories
```

### 📖 Documentação por Camada

- **[🚀 FastDinner.Api](FastDinner.Api/README.md)** - Padrão REST, Middleware, Autenticação JWT
- **[🧠 FastDinner.Application](FastDinner.Application/README.md)** - Padrão CQRS, MediatR, Handlers
- **[🏛️ FastDinner.Domain](FastDinner.Domain/README.md)** - Domain-Driven Design, Entidades, Regras de negócio
- **[🔧 FastDinner.Infrastructure](FastDinner.Infrastructure/README.md)** - Repository Pattern, Entity Framework, Multi-tenancy
- **[📋 FastDinner.Contracts](FastDinner.Contracts/README.md)** - DTOs, Contratos, Validação

## 🍕 Contexto

Imagine que você está criando um sistema de gerenciamento para um restaurante. Aqui, a magia acontece! O nosso sistema permite que os funcionários do restaurante realizem uma série de tarefas emocionantes, como fazer pedidos, gerenciar estoque, processar pagamentos e acompanhar reservas.

## 📋 Requisitos de Prato Principal

### 🍽️ Cardápio
- Adicione pratos ao cardápio com nomes, descrições, preços e categorias
- Organize os pratos em categorias para manter tudo no lugar
- Controle de versão do menu

### 🍽️ Pedidos
- Os clientes podem criar pedidos irresistíveis
- Garçons anotam os pedidos, adicionando pratos do cardápio
- Acompanhe os pedidos desde o início até o momento em que estão prontos

### 📦 Estoque
- Gerencie o estoque de ingredientes e produtos
- O sistema mantém um olho no estoque e atualiza automaticamente
- Controle de produtos por tipo

### 📅 Reservas
- Permita que os clientes reservem mesas
- Especifique data, hora e número de pessoas
- Evite aglomeração, evitando reservas duplicadas

### 💳 Pagamentos
- Funcionários podem registrar pagamentos
- O sistema faz as contas para você
- Aceite pagamentos em dinheiro ou cartão

### 📊 Relatórios
- Gerentes podem criar relatórios mágicos
- Revele vendas diárias, pratos mais populares e estoque baixo
- Os relatórios são apresentados de forma amigável

### 🔐 Autenticação e Autorização
- Funcionários fazem login com diferentes níveis de acesso
- JWT Bearer Token para autenticação
- Controle de acesso baseado em roles

### 🏢 Multi-tenancy
- Suporte a múltiplos restaurantes
- Isolamento completo de dados por tenant
- Configuração dinâmica de banco de dados

## 🛠️ Tecnologias Utilizadas

### Backend
- **.NET 8** - Framework principal
- **Entity Framework Core** - ORM para persistência
- **MediatR** - Padrão Mediator para CQRS
- **JWT Bearer** - Autenticação baseada em tokens
- **StructureMap** - Container IoC customizado
- **Swagger/OpenAPI** - Documentação da API

### Padrões Arquiteturais
- **Clean Architecture** - Separação de responsabilidades
- **Domain-Driven Design** - Modelagem orientada ao domínio
- **CQRS** - Separação de comandos e consultas
- **Repository Pattern** - Abstração do acesso a dados
- **Unit of Work** - Controle de transações

### Banco de Dados
- **SQL Server** - Banco de dados principal
- **Azure Table Storage** - Armazenamento de configurações
- **Entity Framework Migrations** - Controle de versão do banco

## 🚀 Como Executar

### Pré-requisitos
- .NET 8 SDK
- SQL Server (ou SQL Server Express)
- Visual Studio 2022 ou VS Code

### Configuração
1. Clone o repositório
2. Configure a string de conexão no `appsettings.json`
3. Execute as migrations:
   ```bash
   dotnet ef database update --project FastDinner.Infrastructure
   ```
4. Execute a aplicação:
   ```bash
   dotnet run --project FastDinner.Api
   ```

### Docker
```bash
docker-compose up
```

### Endpoints Disponíveis
- **Swagger UI**: `https://localhost:5001/swagger`
- **Health Check**: `https://localhost:5001/`
- **Error Handler**: `https://localhost:5001/error`

## 🔒 Segurança

### Headers Obrigatórios
```http
x-restaurant-id: {restaurant-guid}
x-tenant-name: {tenant-name} (opcional)
Authorization: Bearer {jwt-token}
```

### Validação
- Validação automática de entrada via Data Annotations
- Sanitização de dados
- Controle de acesso baseado em roles

## 📊 Monitoramento

### Logs
- Logs estruturados via ILogger
- Rastreamento de requisições
- Monitoramento de performance

### Métricas
- Tempo de execução de operações
- Taxa de sucesso de transações
- Uso de cache

## 🧪 Testes

### Swagger
- Interface interativa para testes
- Documentação automática
- Exemplos de requisição/resposta

### HTTP Files
- Coleções de teste em `Requests/`
- Exemplos para cada endpoint
- Configurações de ambiente

## 🔄 Integração

### Camadas Conectadas
- **API**: Interface de apresentação REST
- **Application**: Lógica de negócio via CQRS
- **Domain**: Entidades e regras de domínio
- **Infrastructure**: Persistência e serviços externos
- **Contracts**: DTOs e contratos de comunicação

### Comunicação
- **Commands**: Operações de escrita (POST, PUT, DELETE)
- **Queries**: Operações de leitura (GET)
- **Events**: Comunicação assíncrona entre componentes
- **Domain Events**: Notificações de mudanças no domínio

## 📈 Roadmap

### Próximas Funcionalidades
- [ ] Sistema de notificações em tempo real
- [ ] Relatórios avançados com gráficos
- [ ] Integração com sistemas de pagamento
- [ ] App mobile para clientes
- [ ] Sistema de fidelidade
- [ ] Integração com delivery

### Melhorias Técnicas
- [ ] Implementação de cache distribuído (Redis)
- [ ] Logs centralizados (ELK Stack)
- [ ] Monitoramento com APM
- [ ] Testes automatizados (Unit, Integration, E2E)
- [ ] CI/CD pipeline
- [ ] Containerização completa

## 🤝 Contribuição

1. Fork o projeto
2. Crie uma branch para sua feature (`git checkout -b feature/AmazingFeature`)
3. Commit suas mudanças (`git commit -m 'Add some AmazingFeature'`)
4. Push para a branch (`git push origin feature/AmazingFeature`)
5. Abra um Pull Request

## 📄 Licença

Este projeto está sob a licença MIT. Veja o arquivo `LICENSE` para mais detalhes.

---

Aproveite a jornada gastronômica com o Sistema de Gerenciamento de Restaurante e faça com que a experiência de jantar seja inesquecível! 🍽️🚀

**Bon appétit!** 🍷🍴
