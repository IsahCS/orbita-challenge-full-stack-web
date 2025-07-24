using AutoMapper;
using StudentEnrollmentAPI.DTOs;
using StudentEnrollmentAPI.Models;
using StudentEnrollmentAPI.Repositories;

namespace StudentEnrollmentAPI.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;

        public StudentService(IStudentRepository studentRepository, IMapper mapper)
        {
            _studentRepository = studentRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<StudentResponseDto>> GetAllStudentsAsync()
        {
            var students = await _studentRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<StudentResponseDto>>(students);
        }

        public async Task<StudentResponseDto?> GetStudentByIdAsync(int id)
        {
            var student = await _studentRepository.GetByIdAsync(id);
            return student == null ? null : _mapper.Map<StudentResponseDto>(student);
        }

        public async Task<StudentResponseDto> CreateStudentAsync(StudentCreateDto studentDto)
        {
            if (await _studentRepository.RAExistsAsync(studentDto.RA))
            {
                throw new ArgumentException("RA já está em uso por outro aluno.");
            }

            if (await _studentRepository.CPFExistsAsync(studentDto.CPF))
            {
                throw new ArgumentException("CPF já está em uso por outro aluno.");
            }

            if (await _studentRepository.EmailExistsAsync(studentDto.Email))
            {
                throw new ArgumentException("Email já está em uso por outro aluno.");
            }

            var student = _mapper.Map<Student>(studentDto);
            var createdStudent = await _studentRepository.CreateAsync(student);
            return _mapper.Map<StudentResponseDto>(createdStudent);
        }

        public async Task<StudentResponseDto?> UpdateStudentAsync(int id, StudentUpdateDto studentDto)
        {
            var existingStudent = await _studentRepository.GetByIdAsync(id);
            if (existingStudent == null)
            {
                return null;
            }

            if (await _studentRepository.EmailExistsAsync(studentDto.Email, id))
            {
                throw new ArgumentException("Email já está em uso por outro aluno.");
            }

            existingStudent.Name = studentDto.Name;
            existingStudent.Email = studentDto.Email;
            existingStudent.UpdatedAt = DateTime.UtcNow;

            var updatedStudent = await _studentRepository.UpdateAsync(existingStudent);
            return _mapper.Map<StudentResponseDto>(updatedStudent);
        }

        public async Task<bool> DeleteStudentAsync(int id)
        {
            return await _studentRepository.DeleteAsync(id);
        }
    }
}
