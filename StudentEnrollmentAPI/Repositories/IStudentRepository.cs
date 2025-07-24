using StudentEnrollmentAPI.Models;

namespace StudentEnrollmentAPI.Repositories
{
    public interface IStudentRepository
    {
        Task<IEnumerable<Student>> GetAllAsync();
        Task<Student?> GetByIdAsync(int id);
        Task<Student?> GetByRAAsync(string ra);
        Task<Student?> GetByCPFAsync(string cpf);
        Task<Student?> GetByEmailAsync(string email);
        Task<Student> CreateAsync(Student student);
        Task<Student> UpdateAsync(Student student);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> RAExistsAsync(string ra, int? excludeId = null);
        Task<bool> CPFExistsAsync(string cpf, int? excludeId = null);
        Task<bool> EmailExistsAsync(string email, int? excludeId = null);
    }
}
