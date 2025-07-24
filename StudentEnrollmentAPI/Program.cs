using Microsoft.EntityFrameworkCore;
using StudentEnrollmentAPI.Data;
using StudentEnrollmentAPI.Extensions;
using FluentValidation.AspNetCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });

builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo 
    { 
        Title = "Student Enrollment API", 
        Version = "v1",
        Description = "API para gerenciamento de alunos e matrículas"
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddDatabaseServices(builder.Configuration);
builder.Services.AddApplicationServices();

builder.Services.AddFluentValidationAutoValidation();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Student Enrollment API v1");
        c.RoutePrefix = string.Empty; // para acessar o Swagger na raiz
    });
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

await SeedDatabase(app);

app.Run();

// método para popular o banco com dados de exemplo
async Task SeedDatabase(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    
    //garante que o banco seja criado
    await context.Database.EnsureCreatedAsync();
    
    //verificar se já existem dados
    if (!await context.Students.AnyAsync())
    {
        var students = new[]
        {
            new StudentEnrollmentAPI.Models.Student
            {
                Name = "João Silva",
                Email = "joao.silva@email.com",
                RA = "RA001",
                CPF = "12345678901",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new StudentEnrollmentAPI.Models.Student
            {
                Name = "Maria Santos",
                Email = "maria.santos@email.com",
                RA = "RA002",
                CPF = "98765432100",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new StudentEnrollmentAPI.Models.Student
            {
                Name = "Pedro Oliveira",
                Email = "pedro.oliveira@email.com",
                RA = "RA003",
                CPF = "11122233344",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        };
        
        await context.Students.AddRangeAsync(students);
        await context.SaveChangesAsync();
    }
}
