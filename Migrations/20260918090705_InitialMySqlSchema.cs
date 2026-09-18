using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace student_resource_hub.Migrations
{
    /// <inheritdoc />
    public partial class InitialMySqlSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Universities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LogoUrl = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Universities", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AcademicDepartments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UniversityId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(160)", maxLength: 160, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ImageUrl = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcademicDepartments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AcademicDepartments_Universities_UniversityId",
                        column: x => x.UniversityId,
                        principalTable: "Universities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AcademicSemesters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcademicSemesters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AcademicSemesters_AcademicDepartments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "AcademicDepartments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UniversityId = table.Column<int>(type: "int", nullable: true),
                    AcademicDepartmentId = table.Column<int>(type: "int", nullable: true),
                    AcademicSemesterId = table.Column<int>(type: "int", nullable: true),
                    FullName = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PasswordHash = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Role = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Bio = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Location = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PhoneNumber = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProfilePhotoPath = table.Column<string>(type: "varchar(260)", maxLength: 260, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_AcademicDepartments_AcademicDepartmentId",
                        column: x => x.AcademicDepartmentId,
                        principalTable: "AcademicDepartments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Users_AcademicSemesters_AcademicSemesterId",
                        column: x => x.AcademicSemesterId,
                        principalTable: "AcademicSemesters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Users_Universities_UniversityId",
                        column: x => x.UniversityId,
                        principalTable: "Universities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AttendanceSessions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SessionTitle = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CourseCode = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Department = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SessionDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time(6)", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time(6)", nullable: false),
                    Passcode = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttendanceSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttendanceSessions_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Lectures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AcademicDepartmentId = table.Column<int>(type: "int", nullable: true),
                    AcademicSemesterId = table.Column<int>(type: "int", nullable: true),
                    Title = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SubjectName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CourseCode = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProfessorName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Department = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Semester = table.Column<int>(type: "int", nullable: false),
                    Topics = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FilePath = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OriginalFileName = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FileType = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FileSizeBytes = table.Column<long>(type: "bigint", nullable: false),
                    UploadedByUserId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    DownloadCount = table.Column<int>(type: "int", nullable: false),
                    ViewCount = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ApprovedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ApprovalNotes = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lectures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lectures_AcademicDepartments_AcademicDepartmentId",
                        column: x => x.AcademicDepartmentId,
                        principalTable: "AcademicDepartments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Lectures_AcademicSemesters_AcademicSemesterId",
                        column: x => x.AcademicSemesterId,
                        principalTable: "AcademicSemesters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Lectures_Users_UploadedByUserId",
                        column: x => x.UploadedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Notes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AcademicDepartmentId = table.Column<int>(type: "int", nullable: true),
                    AcademicSemesterId = table.Column<int>(type: "int", nullable: true),
                    Title = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SubjectName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CourseCode = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProfessorName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Department = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Semester = table.Column<int>(type: "int", nullable: false),
                    FilePath = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OriginalFileName = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FileType = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FileSizeBytes = table.Column<long>(type: "bigint", nullable: false),
                    UploadedByUserId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    PageCount = table.Column<int>(type: "int", nullable: false),
                    DownloadCount = table.Column<int>(type: "int", nullable: false),
                    ViewCount = table.Column<int>(type: "int", nullable: false),
                    AverageRating = table.Column<decimal>(type: "decimal(3,2)", precision: 3, scale: 2, nullable: false),
                    TotalRatings = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ApprovedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ApprovalNotes = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notes_AcademicDepartments_AcademicDepartmentId",
                        column: x => x.AcademicDepartmentId,
                        principalTable: "AcademicDepartments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Notes_AcademicSemesters_AcademicSemesterId",
                        column: x => x.AcademicSemesterId,
                        principalTable: "AcademicSemesters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Notes_Users_UploadedByUserId",
                        column: x => x.UploadedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PastPapers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AcademicDepartmentId = table.Column<int>(type: "int", nullable: true),
                    AcademicSemesterId = table.Column<int>(type: "int", nullable: true),
                    Title = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SubjectName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CourseCode = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProfessorName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Department = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Semester = table.Column<int>(type: "int", nullable: false),
                    FilePath = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OriginalFileName = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FileType = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FileSizeBytes = table.Column<long>(type: "bigint", nullable: false),
                    UploadedByUserId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    DownloadCount = table.Column<int>(type: "int", nullable: false),
                    ViewCount = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ApprovedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ApprovalNotes = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PastPapers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PastPapers_AcademicDepartments_AcademicDepartmentId",
                        column: x => x.AcademicDepartmentId,
                        principalTable: "AcademicDepartments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PastPapers_AcademicSemesters_AcademicSemesterId",
                        column: x => x.AcademicSemesterId,
                        principalTable: "AcademicSemesters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PastPapers_Users_UploadedByUserId",
                        column: x => x.UploadedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "StudySessions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(160)", maxLength: 160, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudySessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudySessions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AttendanceRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AttendanceSessionId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    StudentName = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StudentEmail = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Role = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SubmittedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttendanceRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttendanceRecords_AttendanceSessions_AttendanceSessionId",
                        column: x => x.AttendanceSessionId,
                        principalTable: "AttendanceSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AttendanceRecords_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "StudyFolders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    StudySessionId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudyFolders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudyFolders_StudySessions_StudySessionId",
                        column: x => x.StudySessionId,
                        principalTable: "StudySessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "StudyResources",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    StudySessionId = table.Column<int>(type: "int", nullable: false),
                    StudyFolderId = table.Column<int>(type: "int", nullable: true),
                    ResourceType = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ResourceId = table.Column<int>(type: "int", nullable: false),
                    ResourceTitle = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CourseCode = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AddedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudyResources", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudyResources_StudyFolders_StudyFolderId",
                        column: x => x.StudyFolderId,
                        principalTable: "StudyFolders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_StudyResources_StudySessions_StudySessionId",
                        column: x => x.StudySessionId,
                        principalTable: "StudySessions",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Universities",
                columns: new[] { "Id", "IsActive", "LogoUrl", "Name" },
                values: new object[] { 1, true, null, "Default University" });

            migrationBuilder.InsertData(
                table: "AcademicDepartments",
                columns: new[] { "Id", "ImageUrl", "IsActive", "Name", "UniversityId" },
                values: new object[,]
                {
                    { 1, "https://images.unsplash.com/photo-1518770660439-4636190af475?auto=format&fit=crop&w=900&q=80", true, "Computer Science and Engineering", 1 },
                    { 2, "https://images.unsplash.com/photo-1581092160607-ee22621dd758?auto=format&fit=crop&w=900&q=80", true, "Mechanical Engineering", 1 },
                    { 3, "https://images.unsplash.com/photo-1504917595217-d4dc5ebe6122?auto=format&fit=crop&w=900&q=80", true, "Industrial and Production Engineering", 1 },
                    { 4, "https://images.unsplash.com/photo-1503387762-592deb58ef4e?auto=format&fit=crop&w=900&q=80", true, "Civil Engineering", 1 },
                    { 5, "https://images.unsplash.com/photo-1551288049-bebda4e38f71?auto=format&fit=crop&w=900&q=80", true, "Data Science", 1 },
                    { 6, "https://images.unsplash.com/photo-1677442136019-21780ecad995?auto=format&fit=crop&w=900&q=80", true, "Artificial Intelligence", 1 },
                    { 7, "https://images.unsplash.com/photo-1436491865332-7a61a109cc05?auto=format&fit=crop&w=900&q=80", true, "Aeronautical Engineering", 1 },
                    { 8, "https://images.unsplash.com/photo-1518773553398-650c184e0bb3?auto=format&fit=crop&w=900&q=80", true, "Software Engineering", 1 }
                });

            migrationBuilder.InsertData(
                table: "AcademicSemesters",
                columns: new[] { "Id", "DepartmentId", "IsActive", "Name", "SortOrder" },
                values: new object[,]
                {
                    { 11, 1, true, "Semester 1", 1 },
                    { 12, 1, true, "Semester 2", 2 },
                    { 13, 1, true, "Semester 3", 3 },
                    { 14, 1, true, "Semester 4", 4 },
                    { 15, 1, true, "Semester 5", 5 },
                    { 16, 1, true, "Semester 6", 6 },
                    { 17, 1, true, "Semester 7", 7 },
                    { 18, 1, true, "Semester 8", 8 },
                    { 21, 2, true, "Semester 1", 1 },
                    { 22, 2, true, "Semester 2", 2 },
                    { 23, 2, true, "Semester 3", 3 },
                    { 24, 2, true, "Semester 4", 4 },
                    { 25, 2, true, "Semester 5", 5 },
                    { 26, 2, true, "Semester 6", 6 },
                    { 27, 2, true, "Semester 7", 7 },
                    { 28, 2, true, "Semester 8", 8 },
                    { 31, 3, true, "Semester 1", 1 },
                    { 32, 3, true, "Semester 2", 2 },
                    { 33, 3, true, "Semester 3", 3 },
                    { 34, 3, true, "Semester 4", 4 },
                    { 35, 3, true, "Semester 5", 5 },
                    { 36, 3, true, "Semester 6", 6 },
                    { 37, 3, true, "Semester 7", 7 },
                    { 38, 3, true, "Semester 8", 8 },
                    { 41, 4, true, "Semester 1", 1 },
                    { 42, 4, true, "Semester 2", 2 },
                    { 43, 4, true, "Semester 3", 3 },
                    { 44, 4, true, "Semester 4", 4 },
                    { 45, 4, true, "Semester 5", 5 },
                    { 46, 4, true, "Semester 6", 6 },
                    { 47, 4, true, "Semester 7", 7 },
                    { 48, 4, true, "Semester 8", 8 },
                    { 51, 5, true, "Semester 1", 1 },
                    { 52, 5, true, "Semester 2", 2 },
                    { 53, 5, true, "Semester 3", 3 },
                    { 54, 5, true, "Semester 4", 4 },
                    { 55, 5, true, "Semester 5", 5 },
                    { 56, 5, true, "Semester 6", 6 },
                    { 57, 5, true, "Semester 7", 7 },
                    { 58, 5, true, "Semester 8", 8 },
                    { 61, 6, true, "Semester 1", 1 },
                    { 62, 6, true, "Semester 2", 2 },
                    { 63, 6, true, "Semester 3", 3 },
                    { 64, 6, true, "Semester 4", 4 },
                    { 65, 6, true, "Semester 5", 5 },
                    { 66, 6, true, "Semester 6", 6 },
                    { 67, 6, true, "Semester 7", 7 },
                    { 68, 6, true, "Semester 8", 8 },
                    { 71, 7, true, "Semester 1", 1 },
                    { 72, 7, true, "Semester 2", 2 },
                    { 73, 7, true, "Semester 3", 3 },
                    { 74, 7, true, "Semester 4", 4 },
                    { 75, 7, true, "Semester 5", 5 },
                    { 76, 7, true, "Semester 6", 6 },
                    { 77, 7, true, "Semester 7", 7 },
                    { 78, 7, true, "Semester 8", 8 },
                    { 81, 8, true, "Semester 1", 1 },
                    { 82, 8, true, "Semester 2", 2 },
                    { 83, 8, true, "Semester 3", 3 },
                    { 84, 8, true, "Semester 4", 4 },
                    { 85, 8, true, "Semester 5", 5 },
                    { 86, 8, true, "Semester 6", 6 },
                    { 87, 8, true, "Semester 7", 7 },
                    { 88, 8, true, "Semester 8", 8 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AcademicDepartments_UniversityId_Name",
                table: "AcademicDepartments",
                columns: new[] { "UniversityId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AcademicSemesters_DepartmentId_Name",
                table: "AcademicSemesters",
                columns: new[] { "DepartmentId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceRecords_AttendanceSessionId_UserId",
                table: "AttendanceRecords",
                columns: new[] { "AttendanceSessionId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceRecords_UserId",
                table: "AttendanceRecords",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceSessions_CourseCode",
                table: "AttendanceSessions",
                column: "CourseCode");

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceSessions_CreatedByUserId",
                table: "AttendanceSessions",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceSessions_IsActive",
                table: "AttendanceSessions",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceSessions_SessionDate",
                table: "AttendanceSessions",
                column: "SessionDate");

            migrationBuilder.CreateIndex(
                name: "IX_Lectures_AcademicDepartmentId",
                table: "Lectures",
                column: "AcademicDepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Lectures_AcademicSemesterId",
                table: "Lectures",
                column: "AcademicSemesterId");

            migrationBuilder.CreateIndex(
                name: "IX_Lectures_CreatedDate",
                table: "Lectures",
                column: "CreatedDate");

            migrationBuilder.CreateIndex(
                name: "IX_Lectures_Department_Year_Semester",
                table: "Lectures",
                columns: new[] { "Department", "Year", "Semester" });

            migrationBuilder.CreateIndex(
                name: "IX_Lectures_Status",
                table: "Lectures",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Lectures_UploadedByUserId",
                table: "Lectures",
                column: "UploadedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_AcademicDepartmentId",
                table: "Notes",
                column: "AcademicDepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_AcademicSemesterId",
                table: "Notes",
                column: "AcademicSemesterId");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_AverageRating",
                table: "Notes",
                column: "AverageRating");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_CreatedDate",
                table: "Notes",
                column: "CreatedDate");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_Department_Year_Semester",
                table: "Notes",
                columns: new[] { "Department", "Year", "Semester" });

            migrationBuilder.CreateIndex(
                name: "IX_Notes_Status",
                table: "Notes",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_UploadedByUserId",
                table: "Notes",
                column: "UploadedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PastPapers_AcademicDepartmentId",
                table: "PastPapers",
                column: "AcademicDepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_PastPapers_AcademicSemesterId",
                table: "PastPapers",
                column: "AcademicSemesterId");

            migrationBuilder.CreateIndex(
                name: "IX_PastPapers_CreatedDate",
                table: "PastPapers",
                column: "CreatedDate");

            migrationBuilder.CreateIndex(
                name: "IX_PastPapers_Department_Year_Semester",
                table: "PastPapers",
                columns: new[] { "Department", "Year", "Semester" });

            migrationBuilder.CreateIndex(
                name: "IX_PastPapers_Status",
                table: "PastPapers",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_PastPapers_UploadedByUserId",
                table: "PastPapers",
                column: "UploadedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_StudyFolders_StudySessionId_Name",
                table: "StudyFolders",
                columns: new[] { "StudySessionId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudyResources_StudyFolderId",
                table: "StudyResources",
                column: "StudyFolderId");

            migrationBuilder.CreateIndex(
                name: "IX_StudyResources_StudySessionId_ResourceType_ResourceId",
                table: "StudyResources",
                columns: new[] { "StudySessionId", "ResourceType", "ResourceId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudySessions_UserId_Name",
                table: "StudySessions",
                columns: new[] { "UserId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Universities_Name",
                table: "Universities",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_AcademicDepartmentId",
                table: "Users",
                column: "AcademicDepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_AcademicSemesterId",
                table: "Users",
                column: "AcademicSemesterId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_UniversityId",
                table: "Users",
                column: "UniversityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttendanceRecords");

            migrationBuilder.DropTable(
                name: "Lectures");

            migrationBuilder.DropTable(
                name: "Notes");

            migrationBuilder.DropTable(
                name: "PastPapers");

            migrationBuilder.DropTable(
                name: "StudyResources");

            migrationBuilder.DropTable(
                name: "AttendanceSessions");

            migrationBuilder.DropTable(
                name: "StudyFolders");

            migrationBuilder.DropTable(
                name: "StudySessions");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "AcademicSemesters");

            migrationBuilder.DropTable(
                name: "AcademicDepartments");

            migrationBuilder.DropTable(
                name: "Universities");
        }
    }
}
