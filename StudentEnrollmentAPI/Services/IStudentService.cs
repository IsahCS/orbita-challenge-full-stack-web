using StudentEnrollmentAPI.DTOs;

namespace StudentEnrollmentAPI.Services
{
    public interface IStudentService
    {
        Task<IEnumerable<StudentResponseDto>> GetAllStudentsAsync();
        Task<StudentResponseDto?> GetStudentByIdAsync(int id);
        Task<StudentResponseDto> CreateStudentAsync(StudentCreateDto studentDto);
        Task<StudentResponseDto?> UpdateStudentAsync(int id, StudentUpdateDto studentDto);
        Task<bool> DeleteStudentAsync(int id);
    }
}
