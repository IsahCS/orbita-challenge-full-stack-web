# Configuração de Banco de Dados

Este arquivo contém as diferentes opções de configuração de banco de dados para o projeto StudentEnrollmentAPI.

## Como usar

1. Escolha a opção de banco desejada
2. Substitua a connection string no arquivo `appsettings.json`
3. Atualize o `DatabaseProvider` correspondente
4. O arquivo `ServiceCollectionExtensions.cs` já está configurado para todos os providers

**Nota**: Todos os pacotes necessários já estão instalados e funcionais.

## Opções de Connection String

### 1. SQL Server Local (LocalDB)
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=StudentEnrollmentDB;Trusted_Connection=true;MultipleActiveResultSets=true"
  },
  "DatabaseProvider": "SQLServer"
}
```

### 2. SQL Server com autenticação Windows
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=StudentEnrollmentDB;Trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=true"
  },
  "DatabaseProvider": "SQLServer"
}
```

### 3. SQL Server com usuário e senha
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=StudentEnrollmentDB;User Id=sa;Password=SuaSenha;MultipleActiveResultSets=true;TrustServerCertificate=true"
  },
  "DatabaseProvider": "SQLServer"
}
```

### 4. MySQL
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=StudentEnrollmentDB;Uid=root;Pwd=SuaSenha;"
  },
  "DatabaseProvider": "MySQL"
}
```

### 5. PostgreSQL
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=StudentEnrollmentDB;Username=postgres;Password=SuaSenha"
  },
  "DatabaseProvider": "PostgreSQL"
}
```

### 6. SQLite (banco local em arquivo) - **CONFIGURAÇÃO ATUAL**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=StudentEnrollment.db"
  },
  "DatabaseProvider": "SQLite"
}
```

### 7. InMemory (apenas para desenvolvimento/testes)
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "StudentEnrollmentDB"
  },
  "DatabaseProvider": "InMemory"
}
```

## Pacotes NuGet necessários

**Todos os pacotes já estão instalados e funcionais:**

- **SQL Server**: `Microsoft.EntityFrameworkCore.SqlServer`
- **MySQL**: `Pomelo.EntityFrameworkCore.MySql`
- **PostgreSQL**: `Npgsql.EntityFrameworkCore.PostgreSQL`
- **SQLite**: `Microsoft.EntityFrameworkCore.Sqlite`
- **InMemory**: `Microsoft.EntityFrameworkCore.InMemory`  

Os pacotes já estão instalados. Caso precise reinstalar:

```bash
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Pomelo.EntityFrameworkCore.MySql
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
dotnet add package Microsoft.EntityFrameworkCore.Sqlite
```

## Migrações

Após configurar o banco, você pode criar e aplicar migrações:

```bash
#criar migração inicial
dotnet ef migrations add InitialCreate

#aplicar migrações
dotnet ef database update
```
