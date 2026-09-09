using Microsoft.EntityFrameworkCore;
using student_resource_hub.Models;

namespace student_resource_hub.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<PastPaper> PastPapers => Set<PastPaper>();
        public DbSet<Note> Notes => Set<Note>();
        public DbSet<Lecture> Lectures => Set<Lecture>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(user => user.Id);
                entity.Property(user => user.FullName).HasMaxLength(120).IsRequired();
                entity.Property(user => user.Email).HasMaxLength(256).IsRequired();
                entity.Property(user => user.PasswordHash).HasMaxLength(500).IsRequired();
                entity.Property(user => user.Role).HasMaxLength(20).IsRequired();
                entity.HasIndex(user => user.Email).IsUnique();
            });

            modelBuilder.Entity<PastPaper>(entity =>
            {
                entity.HasKey(paper => paper.Id);
                entity.Property(paper => paper.Title).HasMaxLength(200).IsRequired();
                entity.Property(paper => paper.SubjectName).HasMaxLength(200).IsRequired();
                entity.Property(paper => paper.CourseCode).HasMaxLength(50).IsRequired();
                entity.Property(paper => paper.Description).HasMaxLength(500);
                entity.Property(paper => paper.ProfessorName).HasMaxLength(200).IsRequired();
                entity.Property(paper => paper.FilePath).HasMaxLength(500).IsRequired();
                entity.Property(paper => paper.OriginalFileName).HasMaxLength(255).IsRequired();
                entity.Property(paper => paper.FileType).HasMaxLength(100);
                entity.Property(paper => paper.ApprovalNotes).HasMaxLength(500);
                entity.HasOne(paper => paper.UploadedByUser)
                    .WithMany()
                    .HasForeignKey(paper => paper.UploadedByUserId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasIndex(paper => new { paper.Department, paper.Year, paper.Semester });
                entity.HasIndex(paper => paper.Status);
                entity.HasIndex(paper => paper.CreatedDate);
            });

            modelBuilder.Entity<Note>(entity =>
            {
                entity.HasKey(note => note.Id);
                entity.Property(note => note.Title).HasMaxLength(200).IsRequired();
                entity.Property(note => note.SubjectName).HasMaxLength(200).IsRequired();
                entity.Property(note => note.CourseCode).HasMaxLength(50).IsRequired();
                entity.Property(note => note.Description).HasMaxLength(1000);
                entity.Property(note => note.ProfessorName).HasMaxLength(200);
                entity.Property(note => note.FilePath).HasMaxLength(500).IsRequired();
                entity.Property(note => note.OriginalFileName).HasMaxLength(255).IsRequired();
                entity.Property(note => note.FileType).HasMaxLength(100);
                entity.Property(note => note.ApprovalNotes).HasMaxLength(500);
                entity.Property(note => note.AverageRating).HasPrecision(3, 2);
                entity.HasOne(note => note.UploadedByUser)
                    .WithMany()
                    .HasForeignKey(note => note.UploadedByUserId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasIndex(note => new { note.Department, note.Year, note.Semester });
                entity.HasIndex(note => note.Status);
                entity.HasIndex(note => note.CreatedDate);
                entity.HasIndex(note => note.AverageRating);
            });

            modelBuilder.Entity<Lecture>(entity =>
            {
                entity.HasKey(lecture => lecture.Id);
                entity.Property(lecture => lecture.Title).HasMaxLength(200).IsRequired();
                entity.Property(lecture => lecture.SubjectName).HasMaxLength(200).IsRequired();
                entity.Property(lecture => lecture.CourseCode).HasMaxLength(50).IsRequired();
                entity.Property(lecture => lecture.Description).HasMaxLength(1000);
                entity.Property(lecture => lecture.ProfessorName).HasMaxLength(200);
                entity.Property(lecture => lecture.Topics).HasMaxLength(100);
                entity.Property(lecture => lecture.FilePath).HasMaxLength(500).IsRequired();
                entity.Property(lecture => lecture.OriginalFileName).HasMaxLength(255).IsRequired();
                entity.Property(lecture => lecture.FileType).HasMaxLength(100);
                entity.Property(lecture => lecture.ApprovalNotes).HasMaxLength(500);
                entity.HasOne(lecture => lecture.UploadedByUser)
                    .WithMany()
                    .HasForeignKey(lecture => lecture.UploadedByUserId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasIndex(lecture => new { lecture.Department, lecture.Year, lecture.Semester });
                entity.HasIndex(lecture => lecture.Status);
                entity.HasIndex(lecture => lecture.CreatedDate);
            });
        }
    }
}