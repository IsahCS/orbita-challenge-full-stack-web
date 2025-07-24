using Xunit;
using Microsoft.EntityFrameworkCore;
using StudentEnrollmentAPI.Data;
using StudentEnrollmentAPI.Models;
using StudentEnrollmentAPI.Repositories;

namespace StudentEnrollmentAPI.Tests.Repositories
{
    public class StudentRepositoryTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly StudentRepository _repository;

        public StudentRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _repository = new StudentRepository(_context);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllStudents_OrderedByName()
        {
            var students = new List<Student>
            {
                new Student { Name = "Maria", Email = "maria@test.com", RA = "RA002", CPF = "98765432100" },
                new Student { Name = "João", Email = "joao@test.com", RA = "RA001", CPF = "12345678901" },
                new Student { Name = "Ana", Email = "ana@test.com", RA = "RA003", CPF = "11111111111" }
            };

            await _context.Students.AddRangeAsync(students);
            await _context.SaveChangesAsync();

            var result = await _repository.GetAllAsync();

            Assert.Equal(3, result.Count());
            Assert.Equal("Ana", result.First().Name);
            Assert.Equal("Maria", result.Last().Name);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnStudent_WhenExists()
        {
            var student = new Student 
            { 
                Name = "João", 
                Email = "joao@test.com", 
                RA = "RA001", 
                CPF = "12345678901" 
            };
            await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();

            var result = await _repository.GetByIdAsync(student.Id);

            Assert.NotNull(result);
            Assert.Equal("João", result.Name);
        }

        [Fact]
        public async Task GetByRAAsync_ShouldReturnStudent_WhenExists()
        {
            var student = new Student 
            { 
                Name = "João", 
                Email = "joao@test.com", 
                RA = "RA001", 
                CPF = "12345678901" 
            };
            await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();

            var result = await _repository.GetByRAAsync("RA001");

            Assert.NotNull(result);
            Assert.Equal("João", result.Name);
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateStudent()
        {
            var student = new Student 
            { 
                Name = "João", 
                Email = "joao@test.com", 
                RA = "RA001", 
                CPF = "12345678901" 
            };

            var result = await _repository.CreateAsync(student);
               
            Assert.NotNull(result);
            Assert.True(result.Id > 0);
            Assert.Equal("João", result.Name);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateStudent()
        {
            var student = new Student 
            { 
                Name = "João", 
                Email = "joao@test.com", 
                RA = "RA001", 
                CPF = "12345678901" 
            };
            await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();

            student.Name = "João Silva";
            var result = await _repository.UpdateAsync(student);

            Assert.Equal("João Silva", result.Name);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteStudent_WhenExists()
        {
            var student = new Student 
            { 
                Name = "João", 
                Email = "joao@test.com", 
                RA = "RA001", 
                CPF = "12345678901" 
            };
            await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();

            var result = await _repository.DeleteAsync(student.Id);

            Assert.True(result);
            var deletedStudent = await _repository.GetByIdAsync(student.Id);
            Assert.Null(deletedStudent);
        }

        [Fact]
        public async Task RAExistsAsync_ShouldReturnTrue_WhenRAExists()
        {
            var student = new Student 
            { 
                Name = "João", 
                Email = "joao@test.com", 
                RA = "RA001", 
                CPF = "12345678901" 
            };
            await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();

            var result = await _repository.RAExistsAsync("RA001");

            Assert.True(result);
        }

        [Fact]
        public async Task RAExistsAsync_ShouldReturnFalse_WhenRANotExists()
        {
            var result = await _repository.RAExistsAsync("RA999");

            Assert.False(result);
        }

        [Fact]
        public async Task CPFExistsAsync_ShouldReturnTrue_WhenCPFExists()
        {
            var student = new Student 
            { 
                Name = "João", 
                Email = "joao@test.com", 
                RA = "RA001", 
                CPF = "12345678901" 
            };
            await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();

            var result = await _repository.CPFExistsAsync("12345678901");

            Assert.True(result);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
