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
        public DbSet<AttendanceSession> AttendanceSessions => Set<AttendanceSession>();
        public DbSet<AttendanceRecord> AttendanceRecords => Set<AttendanceRecord>();
        public DbSet<University> Universities => Set<University>();
        public DbSet<AcademicDepartment> AcademicDepartments => Set<AcademicDepartment>();
        public DbSet<AcademicSemester> AcademicSemesters => Set<AcademicSemester>();
        public DbSet<StudySession> StudySessions => Set<StudySession>();
        public DbSet<StudyFolder> StudyFolders => Set<StudyFolder>();
        public DbSet<StudyResource> StudyResources => Set<StudyResource>();

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
                entity.HasOne(user => user.University).WithMany(university => university.Users).HasForeignKey(user => user.UniversityId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(user => user.AcademicDepartment).WithMany().HasForeignKey(user => user.AcademicDepartmentId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(user => user.AcademicSemester).WithMany().HasForeignKey(user => user.AcademicSemesterId).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<University>(entity =>
            {
                entity.HasKey(university => university.Id);
                entity.Property(university => university.Name).HasMaxLength(200).IsRequired();
                entity.Property(university => university.LogoUrl).HasMaxLength(500);
                entity.HasIndex(university => university.Name).IsUnique();
            });

            modelBuilder.Entity<AcademicDepartment>(entity =>
            {
                entity.HasKey(department => department.Id);
                entity.Property(department => department.Name).HasMaxLength(160).IsRequired();
                entity.Property(department => department.ImageUrl).HasMaxLength(500);
                entity.HasOne(department => department.University).WithMany(university => university.Departments).HasForeignKey(department => department.UniversityId).OnDelete(DeleteBehavior.Cascade);
                entity.HasIndex(department => new { department.UniversityId, department.Name }).IsUnique();
            });

            modelBuilder.Entity<AcademicSemester>(entity =>
            {
                entity.HasKey(semester => semester.Id);
                entity.Property(semester => semester.Name).HasMaxLength(80).IsRequired();
                entity.HasOne(semester => semester.Department).WithMany(department => department.Semesters).HasForeignKey(semester => semester.DepartmentId).OnDelete(DeleteBehavior.Cascade);
                entity.HasIndex(semester => new { semester.DepartmentId, semester.Name }).IsUnique();
            });

            modelBuilder.Entity<StudySession>(entity =>
            {
                entity.HasKey(session => session.Id);
                entity.Property(session => session.Name).HasMaxLength(160).IsRequired();
                entity.Property(session => session.Description).HasMaxLength(500);
                entity.HasOne(session => session.User).WithMany().HasForeignKey(session => session.UserId).OnDelete(DeleteBehavior.Cascade);
                entity.HasIndex(session => new { session.UserId, session.Name }).IsUnique();
            });

            modelBuilder.Entity<StudyFolder>(entity =>
            {
                entity.HasKey(folder => folder.Id);
                entity.Property(folder => folder.Name).HasMaxLength(120).IsRequired();
                entity.HasOne(folder => folder.StudySession).WithMany(session => session.Folders).HasForeignKey(folder => folder.StudySessionId).OnDelete(DeleteBehavior.Cascade);
                entity.HasIndex(folder => new { folder.StudySessionId, folder.Name }).IsUnique();
            });

            modelBuilder.Entity<StudyResource>(entity =>
            {
                entity.HasKey(resource => resource.Id);
                entity.Property(resource => resource.ResourceType).HasMaxLength(20).IsRequired();
                entity.Property(resource => resource.ResourceTitle).HasMaxLength(200).IsRequired();
                entity.Property(resource => resource.CourseCode).HasMaxLength(50);
                entity.HasOne(resource => resource.StudySession).WithMany(session => session.Resources).HasForeignKey(resource => resource.StudySessionId).OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(resource => resource.StudyFolder).WithMany(folder => folder.Resources).HasForeignKey(resource => resource.StudyFolderId).OnDelete(DeleteBehavior.SetNull);
                entity.HasIndex(resource => new { resource.StudySessionId, resource.ResourceType, resource.ResourceId }).IsUnique();
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
                entity.HasOne(paper => paper.AcademicDepartment).WithMany().HasForeignKey(paper => paper.AcademicDepartmentId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(paper => paper.AcademicSemester).WithMany().HasForeignKey(paper => paper.AcademicSemesterId).OnDelete(DeleteBehavior.Restrict);
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
                entity.HasOne(note => note.AcademicDepartment).WithMany().HasForeignKey(note => note.AcademicDepartmentId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(note => note.AcademicSemester).WithMany().HasForeignKey(note => note.AcademicSemesterId).OnDelete(DeleteBehavior.Restrict);
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
                entity.HasOne(lecture => lecture.AcademicDepartment).WithMany().HasForeignKey(lecture => lecture.AcademicDepartmentId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(lecture => lecture.AcademicSemester).WithMany().HasForeignKey(lecture => lecture.AcademicSemesterId).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<AttendanceSession>(entity =>
            {
                entity.HasKey(s => s.Id);
                entity.Property(s => s.SessionTitle).HasMaxLength(200).IsRequired();
                entity.Property(s => s.CourseCode).HasMaxLength(50).IsRequired();
                entity.Property(s => s.Department).HasMaxLength(100).IsRequired();
                entity.Property(s => s.Passcode).HasMaxLength(50).IsRequired();
                entity.HasOne(s => s.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(s => s.CreatedByUserId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasMany(s => s.AttendanceRecords)
                    .WithOne(r => r.AttendanceSession)
                    .HasForeignKey(r => r.AttendanceSessionId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasIndex(s => s.SessionDate);
                entity.HasIndex(s => s.CourseCode);
                entity.HasIndex(s => s.IsActive);
            });

            modelBuilder.Entity<AttendanceRecord>(entity =>
            {
                entity.HasKey(r => r.Id);
                entity.Property(r => r.StudentName).HasMaxLength(120).IsRequired();
                entity.Property(r => r.StudentEmail).HasMaxLength(256).IsRequired();
                entity.Property(r => r.Role).HasMaxLength(20).IsRequired();
                entity.Property(r => r.Status).HasMaxLength(20).IsRequired();
                entity.HasOne(r => r.User)
                    .WithMany()
                    .HasForeignKey(r => r.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasIndex(r => new { r.AttendanceSessionId, r.UserId }).IsUnique();
            });

            var departmentNames = new[]
            {
                "Computer Science and Engineering",
                "Mechanical Engineering",
                "Industrial and Production Engineering",
                "Civil Engineering",
                "Data Science",
                "Artificial Intelligence",
                "Aeronautical Engineering",
                "Software Engineering"
            };
            var departmentImages = new[]
            {
                "https://images.unsplash.com/photo-1518770660439-4636190af475?auto=format&fit=crop&w=900&q=80",
                "https://images.unsplash.com/photo-1581092160607-ee22621dd758?auto=format&fit=crop&w=900&q=80",
                "https://images.unsplash.com/photo-1504917595217-d4dc5ebe6122?auto=format&fit=crop&w=900&q=80",
                "https://images.unsplash.com/photo-1503387762-592deb58ef4e?auto=format&fit=crop&w=900&q=80",
                "https://images.unsplash.com/photo-1551288049-bebda4e38f71?auto=format&fit=crop&w=900&q=80",
                "https://images.unsplash.com/photo-1677442136019-21780ecad995?auto=format&fit=crop&w=900&q=80",
                "https://images.unsplash.com/photo-1436491865332-7a61a109cc05?auto=format&fit=crop&w=900&q=80",
                "https://images.unsplash.com/photo-1518773553398-650c184e0bb3?auto=format&fit=crop&w=900&q=80"
            };
            modelBuilder.Entity<University>().HasData(new University { Id = 1, Name = "Default University", IsActive = true });
            modelBuilder.Entity<AcademicDepartment>().HasData(departmentNames.Select((name, index) => new AcademicDepartment
            {
                Id = index + 1,
                UniversityId = 1,
                Name = name,
                ImageUrl = departmentImages[index],
                IsActive = true
            }));
            modelBuilder.Entity<AcademicSemester>().HasData(
                Enumerable.Range(1, 8).SelectMany(departmentId => new[]
                {
                    new AcademicSemester { Id = departmentId * 10 + 1, DepartmentId = departmentId, Name = "Semester 1", SortOrder = 1, IsActive = true },
                    new AcademicSemester { Id = departmentId * 10 + 2, DepartmentId = departmentId, Name = "Semester 2", SortOrder = 2, IsActive = true },
                    new AcademicSemester { Id = departmentId * 10 + 3, DepartmentId = departmentId, Name = "Semester 3", SortOrder = 3, IsActive = true },
                    new AcademicSemester { Id = departmentId * 10 + 4, DepartmentId = departmentId, Name = "Semester 4", SortOrder = 4, IsActive = true },
                    new AcademicSemester { Id = departmentId * 10 + 5, DepartmentId = departmentId, Name = "Semester 5", SortOrder = 5, IsActive = true },
                    new AcademicSemester { Id = departmentId * 10 + 6, DepartmentId = departmentId, Name = "Semester 6", SortOrder = 6, IsActive = true },
                    new AcademicSemester { Id = departmentId * 10 + 7, DepartmentId = departmentId, Name = "Semester 7", SortOrder = 7, IsActive = true },
                    new AcademicSemester { Id = departmentId * 10 + 8, DepartmentId = departmentId, Name = "Semester 8", SortOrder = 8, IsActive = true }
                }));
        }
    }
}