using System.ComponentModel.DataAnnotations;

namespace StudentEnrollmentAPI.Models
{
    public class Student
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        [StringLength(20)]
        public string RA { get; set; } = string.Empty;
        
        [Required]
        [StringLength(11, MinimumLength = 11)]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "CPF deve conter 11 dígitos")]
        public string CPF { get; set; } = string.Empty;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
