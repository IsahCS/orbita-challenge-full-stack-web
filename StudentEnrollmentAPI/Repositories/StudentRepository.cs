using Microsoft.EntityFrameworkCore;
using StudentEnrollmentAPI.Data;
using StudentEnrollmentAPI.Models;

namespace StudentEnrollmentAPI.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly ApplicationDbContext _context;

        public StudentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Student>> GetAllAsync()
        {
            return await _context.Students
                .OrderBy(s => s.Name)
                .ToListAsync();
        }

        public async Task<Student?> GetByIdAsync(int id)
        {
            return await _context.Students
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Student?> GetByRAAsync(string ra)
        {
            return await _context.Students
                .FirstOrDefaultAsync(s => s.RA == ra);
        }

        public async Task<Student?> GetByCPFAsync(string cpf)
        {
            return await _context.Students
                .FirstOrDefaultAsync(s => s.CPF == cpf);
        }

        public async Task<Student?> GetByEmailAsync(string email)
        {
            return await _context.Students
                .FirstOrDefaultAsync(s => s.Email == email);
        }

        public async Task<Student> CreateAsync(Student student)
        {
            student.CreatedAt = DateTime.UtcNow;
            student.UpdatedAt = DateTime.UtcNow;
            
            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            return student;
        }

        public async Task<Student> UpdateAsync(Student student)
        {
            student.UpdatedAt = DateTime.UtcNow;
            
            _context.Students.Update(student);
            await _context.SaveChangesAsync();
            return student;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var student = await GetByIdAsync(id);
            if (student == null)
                return false;

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Students
                .AnyAsync(s => s.Id == id);
        }

        public async Task<bool> RAExistsAsync(string ra, int? excludeId = null)
        {
            return await _context.Students
                .AnyAsync(s => s.RA == ra && (excludeId == null || s.Id != excludeId));
        }

        public async Task<bool> CPFExistsAsync(string cpf, int? excludeId = null)
        {
            return await _context.Students
                .AnyAsync(s => s.CPF == cpf && (excludeId == null || s.Id != excludeId));
        }

        public async Task<bool> EmailExistsAsync(string email, int? excludeId = null)
        {
            return await _context.Students
                .AnyAsync(s => s.Email == email && (excludeId == null || s.Id != excludeId));
        }
    }
}
