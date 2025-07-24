using Microsoft.EntityFrameworkCore;
using StudentEnrollmentAPI.Models;

namespace StudentEnrollmentAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        
        public DbSet<Student> Students { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(e => e.Id);
                
                entity.HasIndex(e => e.RA)
                    .IsUnique()
                    .HasDatabaseName("IX_Student_RA");
                
                entity.HasIndex(e => e.CPF)
                    .IsUnique()
                    .HasDatabaseName("IX_Student_CPF");
                
                entity.HasIndex(e => e.Email)
                    .IsUnique()
                    .HasDatabaseName("IX_Student_Email");
                
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
                
                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100);
                
                entity.Property(e => e.RA)
                    .IsRequired()
                    .HasMaxLength(20);
                
                entity.Property(e => e.CPF)
                    .IsRequired()
                    .HasMaxLength(11);
                
                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");
                
                entity.Property(e => e.UpdatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");
            });
        }
    }
}
