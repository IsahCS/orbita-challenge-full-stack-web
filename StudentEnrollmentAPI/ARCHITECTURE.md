# Documentação Técnica - Sistema de Alunos

## Sobre o projeto

Este é um sistema simples para gerenciar cadastro de alunos. Foi desenvolvido pensando na facilidade de uso e manutenção, seguindo algumas boas práticas que aprendi ao longo do tempo.

## Como está estruturado

O projeto está organizado de forma bem tradicional para APIs .NET:

```
StudentEnrollmentAPI/
├── Controllers/           # Onde ficam os endpoints da API
├── Services/             # A lógica de negócio fica aqui
├── Repositories/         # Acesso ao banco de dados
├── Models/              # As entidades (Student, etc.)
├── DTOs/                # Objetos para transporte de dados
├── Data/                # Configuração do Entity Framework
├── Validators/          # Validações dos dados de entrada
└── Tests/               # Testes (porque teste é importante!)
```

## Principais padrões utilizados

**Repository Pattern**: Usei para abstrair o acesso aos dados. Facilita muito na hora de fazer testes e se precisar trocar de banco no futuro.

**Service Layer**: Toda a lógica de negócio fica nos Services. Os Controllers só recebem as requisições e chamam o Service apropriado.

**Dependency Injection**: O .NET já vem com um container bom, então aproveitei para deixar tudo bem desacoplado.

**DTOs**: Separo os objetos que trafegam na api das entidades do banco. Ajuda na segurança e flexibilidade.

## Tecnologias escolhidas

**Backend:**
- **.NET 9**: A versão mais nova sempre tem suas vantagens de performance
- **Entity Framework Core**: Facilita muito o trabalho com banco de dados
- **FluentValidation**: Prefiro ele ao invés das Data Annotations para validações mais complexas
- **AutoMapper**: Economiza bastante código repetitivo no mapeamento
- **Swagger**: Essencial para documentar a API

**Banco:**
- **In-Memory** para desenvolvimento, rápido e prático para testes
- Preparado para **PostgreSQL** ou **MySQL** em produção

## Endpoints da API

A API é bem direta ao ponto:

**GET /api/students** - Lista todos os alunos
**GET /api/students/{id}** - Pega um aluno específico  
**POST /api/students** - Cadastra um novo aluno
**PUT /api/students/{id}** - Atualiza um aluno existente
**DELETE /api/students/{id}** - Remove um aluno

## Regras de negócio implementadas

**Campos obrigatórios:**
- Nome (entre 2 e 100 caracteres)
- Email (formato válido)
- RA (único no sistema)
- CPF (válido e único)

**Unicidade:**
- Não pode ter dois alunos com mesmo RA
- não pode ter dois alunos com mesmo CPF  
- não pode ter dois alunos com mesmo email

**Campos não editáveis:**
- RA e CPF não podem ser alterados depois de criados
- Apenas nome e email são editáveis

## Estrutura do banco

Por enquanto uso apenas uma tabela Student bem simples:

```sql
Students (
    Id INT PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    RA NVARCHAR(20) NOT NULL,
    CPF NVARCHAR(11) NOT NULL,
    CreatedAt DATETIME2 NOT NULL,
    UpdatedAt DATETIME2 NOT NULL
)
```

Com índices únicos nos campos RA, CPF e Email para garantir unicidade e performance.

## Como rodar

É bem direto:

```bash
dotnet restore
dotnet run
```

Acesse http://localhost:5078 e o Swagger já fica disponível na raiz.

Para rodar os testes:
```bash
dotnet test
```

## Algumas considerações

**Por que in-memory database?**
Para desenvolvimento é muito mais prático. Em produção seria configurado um PostgreSQL ou MySQL real.

**Autenticação?**
Não implementei por enquanto, mas a estrutura permite adicionar facilmente JWT ou outro mecanismo.

**Performance?**
Para o volume esperado de alunos deve ser mais que suficiente. Se crescer muito, algumas otimizações como cache e paginação seriam interessantes.

## Testes

Implementei testes para as principais funcionalidades:
- Service layer (regras de negócio)
- Repository (operações de dados)
- Validações
- Cenários de erro

não é 100% de cobertura, mas cobre os casos mais críticos.
