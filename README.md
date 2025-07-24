# Student Enrollment System

Sistema de gerenciamento de alunos com backend .NET Core e frontend Vue.js + Vuetify.

## Executando o Sistema Completo

### 1. Executar o Backend (.NET Core API)
```bash
cd StudentEnrollmentAPI
dotnet run
```
**API rodará em**: `http://localhost:5078`

### 2. Executar o Frontend (Vue.js + Vuetify)
```bash
cd StudentEnrollmentFront
npm install  #primeira vez apenas
npm run dev
```
**Frontend rodará em**: `http://localhost:3000`

### 3. Acessar a Aplicação
- **Interface Web**: `http://localhost:3000`
- **API Swagger**: `http://localhost:5078`

## Estrutura do Projeto

```
StudentEnrollmentAPI/
├── StudentEnrollmentAPI/     # backend .NET Core
│   ├── Controllers/          # controladores da API
│   ├── Services/            # lógica de negócio
│   ├── Repositories/        # acesso a dados
│   ├── Models/              # entidades
│   ├── DTOs/                # data Transfer Objects
│   ├── Data/                # context EF Core
│   ├── Tests/               # testes unitários
│   └── Program.cs           # configuração da API
└── StudentEnrollmentFrontend/   # frontend Vue.js
    ├── src/
    │   ├── views/           # páginas principais
    │   ├── components/      # componentes reutilizáveis
    │   ├── stores/          # gerenciamento de estado
    │   ├── services/        # integração com API
    │   └── types/           # tipos TypeScript
    ├── package.json
    └── vite.config.ts
```

## Tecnologias

### Backend
- **.NET 9.0**
- **ASP.NET Core Web API**
- **Entity Framework Core** (In-Memory Database)
- **AutoMapper**
- **FluentValidation**
- **Swagger/OpenAPI**
- **xUnit** (testes)

### Frontend
- **Vue.js 3**
- **Vuetify 3**
- **TypeScript**
- **Vite**
- **Pinia** (estado)
- **Vue Router**
- **Axios**

## Funcionalidades Implementadas

### Backend API
- CRUD
- Validações (CPF, Email único, RA único)
- Tratamento de erros
- Documentação Swagger
- Testes unitários
- Arquitetura Clean Code

### Frontend
- Listagem de alunos com busca
- Cadastro de novos alunos
- Edição de alunos (nome e email)
- Exclusão com confirmação
- Validações em tempo real
- Interface responsiva
- Seguindo mockups fornecidos

## Regras de Negócio

### Campos do Aluno
- **Nome**: Editável, obrigatório (2-100 chars)
- **Email**: Editável, obrigatório, único, formato válido
- **RA**: Não editável após criação, único, obrigatório
- **CPF**: Não editável após criação, único, validação brasileira

### Validações
- CPF: Algoritmo de validação brasileiro
- Email: Formato válido e unicidade
- RA: Unicidade no sistema
- Campos obrigatórios validados

## Testando o Sistema

### Testes Backend
```bash
cd StudentEnrollmentAPI
dotnet test
```

### Testes Manuais Frontend
1. Acesse `http://localhost:3000`
2. Use o arquivo `StudentEnrollmentAPI.http` para testar a API
3. Teste todos os cenários:
   - Criar aluno válido
   - Tentar CPF/RA/Email duplicado
   - Editar aluno (apenas Nome/Email)
   - Excluir com confirmação
   - Buscar alunos

## Telas

### Tela de Listagem
- Tabela com todos os alunos
- Campo de busca
- Botão "Cadastrar Aluno"
- Ações por linha (Editar/Excluir)

### Tela de Cadastro/Edição
- Formulário com 4 campos
- Validações em tempo real
- CPF com máscara automática
- RA/CPF desabilitados na edição

## Configurações Importantes

### CORS (Backend)
```csharp
app.UseCors("AllowAll"); // permite frontend local
```

### Proxy (Frontend)
```typescript
proxy: {
  '/api': {
    target: 'http://localhost:5078',
    changeOrigin: true
  }
}
```

## Próximos Passos

### Possíveis melhorias
- Autenticação JWT
- Paginação na listagem
- Filtros
- Upload de foto do aluno
- Relatórios em PDF
- Deploy em produção

### Banco de Dados
- Atualmente: In-Memory (desenvolvimento)
- Produção: Configurar PostgreSQL/MySQL

---