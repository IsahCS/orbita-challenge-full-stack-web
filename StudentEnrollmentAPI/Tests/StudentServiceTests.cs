using Xunit;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StudentEnrollmentAPI.Data;
using StudentEnrollmentAPI.Models;
using StudentEnrollmentAPI.Repositories;
using StudentEnrollmentAPI.Services;
using StudentEnrollmentAPI.DTOs;
using StudentEnrollmentAPI.Mappings;

namespace StudentEnrollmentAPI.Tests.Services
{
    public class StudentServiceTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly StudentRepository _repository;
        private readonly StudentService _service;

        public StudentServiceTests()
        {
            // configura InMemory database
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);

            // configura AutoMapper
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = config.CreateMapper();

            _repository = new StudentRepository(_context);
            _service = new StudentService(_repository, _mapper);
        }

        [Fact]
        public async Task GetAllStudentsAsync_ShouldReturnAllStudents()
        {
            var students = new List<Student>
            {
                new Student { Name = "João", Email = "joao@test.com", RA = "RA001", CPF = "12345678901" },
                new Student { Name = "Maria", Email = "maria@test.com", RA = "RA002", CPF = "98765432100" }
            };

            await _context.Students.AddRangeAsync(students);
            await _context.SaveChangesAsync();

            var result = await _service.GetAllStudentsAsync();

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetStudentByIdAsync_ShouldReturnStudent_WhenExists()
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

            var result = await _service.GetStudentByIdAsync(student.Id);

            Assert.NotNull(result);
            Assert.Equal("João", result.Name);
        }

        [Fact]
        public async Task GetStudentByIdAsync_ShouldReturnNull_WhenNotExists()
        {
            var result = await _service.GetStudentByIdAsync(999);

            Assert.Null(result);
        }

        [Fact]
        public async Task CreateStudentAsync_ShouldCreateStudent_WithValidData()
        {
            var studentDto = new StudentCreateDto
            {
                Name = "João Silva",
                Email = "joao@test.com",
                RA = "RA001",
                CPF = "12345678901"
            };

            var result = await _service.CreateStudentAsync(studentDto);

            Assert.NotNull(result);
            Assert.Equal("João Silva", result.Name);
            Assert.Equal("joao@test.com", result.Email);
        }

        [Fact]
        public async Task CreateStudentAsync_ShouldThrowException_WhenRAExists()
        {
            var existingStudent = new Student 
            { 
                Name = "Maria", 
                Email = "maria@test.com", 
                RA = "RA001", 
                CPF = "98765432100" 
            };
            await _context.Students.AddAsync(existingStudent);
            await _context.SaveChangesAsync();

            var studentDto = new StudentCreateDto
            {
                Name = "João Silva",
                Email = "joao@test.com",
                RA = "RA001", 
                CPF = "12345678901"
            };

            var exception = await Assert.ThrowsAsync<ArgumentException>(
                () => _service.CreateStudentAsync(studentDto)
            );
            Assert.Contains("RA já está em uso", exception.Message);
        }

        [Fact]
        public async Task CreateStudentAsync_ShouldThrowException_WhenCPFExists()
        {
            var existingStudent = new Student 
            { 
                Name = "Maria", 
                Email = "maria@test.com", 
                RA = "RA001", 
                CPF = "12345678901" 
            };
            await _context.Students.AddAsync(existingStudent);
            await _context.SaveChangesAsync();

            var studentDto = new StudentCreateDto
            {
                Name = "João Silva",
                Email = "joao@test.com",
                RA = "RA002",
                CPF = "12345678901" 
            };

            var exception = await Assert.ThrowsAsync<ArgumentException>(
                () => _service.CreateStudentAsync(studentDto)
            );
            Assert.Contains("CPF já está em uso", exception.Message);
        }

        [Fact]
        public async Task UpdateStudentAsync_ShouldUpdateStudent_WithValidData()
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

            var updateDto = new StudentUpdateDto
            {
                Name = "João Silva Atualizado",
                Email = "joao.novo@test.com"
            };

            var result = await _service.UpdateStudentAsync(student.Id, updateDto);

            Assert.NotNull(result);
            Assert.Equal("João Silva Atualizado", result.Name);
            Assert.Equal("joao.novo@test.com", result.Email);
            Assert.Equal("RA001", result.RA); 
        }

        [Fact]
        public async Task UpdateStudentAsync_ShouldReturnNull_WhenStudentNotExists()
        {
            var updateDto = new StudentUpdateDto
            {
                Name = "João Silva",
                Email = "joao@test.com"
            };

            var result = await _service.UpdateStudentAsync(999, updateDto);

            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteStudentAsync_ShouldReturnTrue_WhenStudentExists()
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

            var result = await _service.DeleteStudentAsync(student.Id);

            Assert.True(result);
        }

        [Fact]
        public async Task DeleteStudentAsync_ShouldReturnFalse_WhenStudentNotExists()
        {
            var result = await _service.DeleteStudentAsync(999);

            Assert.False(result);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
