using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace student_resource_hub.Migrations
{
    /// <inheritdoc />
    public partial class AddResourceTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                IF OBJECT_ID(N'dbo.Notes', N'U') IS NULL
                BEGIN
                    CREATE TABLE dbo.Notes
                    (
                        Id int IDENTITY(1,1) NOT NULL CONSTRAINT PK_Notes PRIMARY KEY,
                        Title nvarchar(200) NOT NULL,
                        SubjectName nvarchar(200) NOT NULL,
                        CourseCode nvarchar(50) NOT NULL,
                        Description nvarchar(1000) NULL,
                        ProfessorName nvarchar(200) NULL,
                        Department int NOT NULL,
                        Year int NOT NULL,
                        Semester int NOT NULL,
                        FilePath nvarchar(500) NOT NULL,
                        FileType nvarchar(100) NULL,
                        FileSizeBytes bigint NOT NULL,
                        UploadedByUserId int NOT NULL,
                        Status int NOT NULL,
                        PageCount int NOT NULL,
                        DownloadCount int NOT NULL,
                        ViewCount int NOT NULL,
                        AverageRating decimal(18,2) NOT NULL,
                        TotalRatings int NOT NULL,
                        CreatedDate datetime2 NOT NULL,
                        ModifiedDate datetime2 NULL,
                        ApprovedDate datetime2 NULL,
                        ApprovalNotes nvarchar(500) NULL,
                        CONSTRAINT FK_Notes_Users_UploadedByUserId FOREIGN KEY (UploadedByUserId)
                            REFERENCES dbo.Users (Id)
                    );
                END
                """);

            migrationBuilder.CreateTable(
                name: "PastPapers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SubjectName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CourseCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ProfessorName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Department = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Semester = table.Column<int>(type: "int", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    OriginalFileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FileType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FileSizeBytes = table.Column<long>(type: "bigint", nullable: false),
                    UploadedByUserId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    DownloadCount = table.Column<int>(type: "int", nullable: false),
                    ViewCount = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApprovedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApprovalNotes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PastPapers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PastPapers_Users_UploadedByUserId",
                        column: x => x.UploadedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notes");

            migrationBuilder.DropTable(
                name: "PastPapers");
        }
    }
}
