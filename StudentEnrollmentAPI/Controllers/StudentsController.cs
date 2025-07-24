using Microsoft.AspNetCore.Mvc;
using StudentEnrollmentAPI.DTOs;
using StudentEnrollmentAPI.Services;

namespace StudentEnrollmentAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;
        private readonly ILogger<StudentsController> _logger;

        public StudentsController(IStudentService studentService, ILogger<StudentsController> logger)
        {
            _studentService = studentService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentResponseDto>>> GetAllStudents()
        {
            try
            {
                var students = await _studentService.GetAllStudentsAsync();
                return Ok(new { success = true, data = students });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar alunos");
                return StatusCode(500, new { success = false, message = "Erro interno do servidor" });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StudentResponseDto>> GetStudent(int id)
        {
            try
            {
                var student = await _studentService.GetStudentByIdAsync(id);
                if (student == null)
                {
                    return NotFound(new { success = false, message = "Aluno não encontrado" });
                }

                return Ok(new { success = true, data = student });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar aluno com ID {Id}", id);
                return StatusCode(500, new { success = false, message = "Erro interno do servidor" });
            }
        }

        [HttpPost]
        public async Task<ActionResult<StudentResponseDto>> CreateStudent([FromBody] StudentCreateDto studentDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { 
                        success = false, 
                        message = "Dados inválidos", 
                        errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                    });
                }

                var createdStudent = await _studentService.CreateStudentAsync(studentDto);
                return CreatedAtAction(
                    nameof(GetStudent), 
                    new { id = createdStudent.Id }, 
                    new { success = true, message = "Aluno criado com sucesso", data = createdStudent }
                );
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar aluno");
                return StatusCode(500, new { success = false, message = "Erro interno do servidor" });
            }
        }
        
        [HttpPut("{id}")]
        public async Task<ActionResult<StudentResponseDto>> UpdateStudent(int id, [FromBody] StudentUpdateDto studentDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { 
                        success = false, 
                        message = "Dados inválidos", 
                        errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                    });
                }

                var updatedStudent = await _studentService.UpdateStudentAsync(id, studentDto);
                if (updatedStudent == null)
                {
                    return NotFound(new { success = false, message = "Aluno não encontrado" });
                }

                return Ok(new { success = true, message = "Aluno atualizado com sucesso", data = updatedStudent });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar aluno com ID {Id}", id);
                return StatusCode(500, new { success = false, message = "Erro interno do servidor" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteStudent(int id)
        {
            try
            {
                var deleted = await _studentService.DeleteStudentAsync(id);
                if (!deleted)
                {
                    return NotFound(new { success = false, message = "Aluno não encontrado" });
                }

                return Ok(new { success = true, message = "Aluno excluído com sucesso" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao excluir aluno com ID {Id}", id);
                return StatusCode(500, new { success = false, message = "Erro interno do servidor" });
            }
        }
    }
}
