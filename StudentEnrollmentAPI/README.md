# Student Enrollment API

Um sistema simples para cadastrar e gerenciar alunos, desenvolvido em .NET 9.

## O que faz

- Cadastrar novos alunos com Nome, Email, RA e CPF
- Listar todos os alunos cadastrados
- Editar dados básicos (nome e email)
- Remover alunos quando necessário
- Validar CPF e evitar duplicações de RA/email

## Como está organizado

- **Controllers**: recebem as requisições HTTP
- **Services**: processam a lógica de negócio
- **Repositories**: fazem acesso ao banco de dados
- **Models**: representam os alunos
- **DTOs**: objetos para entrada/saída da API

### Tecnologias
- **.NET 9.0**: Framework principal
- **Entity Framework Core**: ORM para acesso a dados
- **AutoMapper**: mapeamento objeto-objeto
- **FluentValidation**: validações fluentes
- **Swagger/OpenAPI**: documentação da API
- **xUnit**: Framework de testes

## Campos do Aluno

### Obrigatórios
- **Nome**: entre 2 e 100 caracteres (editável)
- **Email**: formato válido e único (editável)
- **RA**: identificador único (não editável)
- **CPF**: documento válido e único (não editável)

### Regras de Negócio
1. RA deve ser único no sistema
2. CPF deve ser único e válido
3. Email deve ser único e ter formato válido
4. Após criação, apenas Nome e Email podem ser editados
5. RA e CPF são imutáveis após criação

## Como Executar

### Pré-requisitos
- .NET 9.0 SDK
- Visual Studio 2022 ou VS Code

### Executando a Aplicação

1. **Clone o repositório**
```bash
git clone <repository-url>
cd StudentEnrollmentAPI
```

2. **Restaure as dependências**
```bash
dotnet restore
```

3. **Execute a aplicação**
```bash
dotnet run
```

4. **Acesse a aplicação**
- API: `https://localhost:5001`
- Swagger UI: `https://localhost:5001/`

### Executando os Testes
```bash
dotnet test
```

## Endpoints da API

### GET /api/students
Lista todos os alunos cadastrados
```json
{
  "success": true,
  "data": [
    {
      "id": 1,
      "name": "João Silva",
      "email": "joao.silva@email.com",
      "ra": "RA001",
      "cpf": "12345678901",
      "createdAt": "2025-01-22T10:00:00Z",
      "updatedAt": "2025-01-22T10:00:00Z"
    }
  ]
}
```

### GET /api/students/{id}
Busca um aluno específico
```json
{
  "success": true,
  "data": {
    "id": 1,
    "name": "João Silva",
    "email": "joao.silva@email.com",
    "ra": "RA001",
    "cpf": "12345678901",
    "createdAt": "2025-01-22T10:00:00Z",
    "updatedAt": "2025-01-22T10:00:00Z"
  }
}
```

### POST /api/students
Cria um novo aluno
**Body:**
```json
{
  "name": "Maria Santos",
  "email": "maria.santos@email.com",
  "ra": "RA002",
  "cpf": "98765432100"
}
```

**Resposta:**
```json
{
  "success": true,
  "message": "Aluno criado com sucesso",
  "data": {
    "id": 2,
    "name": "Maria Santos",
    "email": "maria.santos@email.com",
    "ra": "RA002",
    "cpf": "98765432100",
    "createdAt": "2025-01-22T10:30:00Z",
    "updatedAt": "2025-01-22T10:30:00Z"
  }
}
```

### PUT /api/students/{id}
Atualiza um aluno existente (apenas Nome e Email)
**Body:**
```json
{
  "name": "Maria Santos Silva",
  "email": "maria.santos.silva@email.com"
}
```

### DELETE /api/students/{id}
Remove um aluno
```json
{
  "success": true,
  "message": "Aluno excluído com sucesso"
}
```

## Testes

### Cobertura de Testes
- **StudentService**: Testes de lógica de negócio
- **StudentRepository**: Testes de acesso a dados
- **Validações**: Testes de regras de negócio

### Cenários Testados
- Criação de aluno com dados válidos
- Validação de RA duplicado
- Validação de CPF duplicado
- Validação de Email duplicado
- Atualização de dados editáveis
- Exclusão de aluno
- Busca por ID existente/inexistente

## Estrutura do Projeto

```
StudentEnrollmentAPI/
├── Controllers/
│   └── StudentsController.cs      # Endpoints da API
├── Services/
│   ├── IStudentService.cs         # Interface do serviço
│   └── StudentService.cs          # Lógica de negócio
├── Repositories/
│   ├── IStudentRepository.cs      # Interface do repositório
│   └── StudentRepository.cs       # Acesso a dados
├── Models/
│   └── Student.cs                 # Entidade do domínio
├── DTOs/
│   └── StudentDto.cs             # Data Transfer Objects
├── Data/
│   └── ApplicationDbContext.cs    # Contexto do EF Core
├── Mappings/
│   └── MappingProfile.cs         # Configuração AutoMapper
├── Validators/
│   └── StudentValidators.cs      # Validações FluentValidation
├── Extensions/
│   └── ServiceCollectionExtensions.cs # Configuração DI
├── Tests/
│   ├── StudentServiceTests.cs    # Testes do serviço
│   └── StudentRepositoryTests.cs # Testes do repositório
├── Program.cs                    # Ponto de entrada
├── ARCHITECTURE.md              # Documentação de arquitetura
├── DATABASE_CONFIG.md           # Guia de configuração de banco de dados
└── README.md                    # Este arquivo
```

## Configuração

### Banco de Dados
- **Padrão**: SQLite (banco local em arquivo)
- **Opções disponíveis**: SQL Server, MySQL, PostgreSQL, SQLite, In-Memory
- **Configuração**: Consulte o arquivo `DATABASE_CONFIG.md` para detalhes de configuração de cada banco

### CORS
- **Desenvolvimento**: Permite todas as origens
- **Produção**: Restringir aos domínios específicos

### Logging
- Configurado com ILogger do ASP.NET Core
- Logs estruturados para análise

## Próximos passos

### Possíveis Melhorias
- Autenticação JWT
- Paginação na listagem
- Cache com Redis
- Logs estruturados com Serilog
- Monitoramento com Application Insights
- Deploy automatizado com CI/CD
- Banco de dados SQL Server/PostgreSQL
- Versionamento da API
- Sistema de matrículas em turmas
- Upload de foto do aluno
- Relatórios em PDF
- Notificações por email

## Critérios de aceitação Atendidos

**Cadastrar novo aluno**: Tela/endpoint de cadastro com validações  
**Listar alunos cadastrados**: Endpoint com listagem ordenada  
**Editar cadastro de aluno**: Endpoint de atualização (apenas campos editáveis)  
**Excluir cadastro de aluno**: Endpoint de exclusão  
**Campos obrigatórios**: Nome, Email, RA, CPF implementados  
**Unicidade**: RA único como chave  
**Campos não editáveis**: RA e CPF protegidos  
**Testes unitários**: Cobertura abrangente  
**Documentação**: Arquitetura documentada  

---