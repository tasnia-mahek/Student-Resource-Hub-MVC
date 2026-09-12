using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace student_resource_hub.Migrations
{
    /// <inheritdoc />
    public partial class ReconcileNotesAndLectures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                IF OBJECT_ID(N'dbo.Notes', N'U') IS NOT NULL
                BEGIN
                    IF COL_LENGTH(N'dbo.Notes', N'SubjectName') IS NULL ALTER TABLE dbo.Notes ADD SubjectName nvarchar(200) NOT NULL CONSTRAINT DF_Notes_SubjectName DEFAULT N'';
                    IF COL_LENGTH(N'dbo.Notes', N'CourseCode') IS NULL ALTER TABLE dbo.Notes ADD CourseCode nvarchar(50) NOT NULL CONSTRAINT DF_Notes_CourseCode DEFAULT N'';
                    IF COL_LENGTH(N'dbo.Notes', N'ProfessorName') IS NULL ALTER TABLE dbo.Notes ADD ProfessorName nvarchar(200) NULL;
                    IF COL_LENGTH(N'dbo.Notes', N'Department') IS NULL ALTER TABLE dbo.Notes ADD Department int NOT NULL CONSTRAINT DF_Notes_Department DEFAULT 0;
                    IF COL_LENGTH(N'dbo.Notes', N'Year') IS NULL ALTER TABLE dbo.Notes ADD Year int NOT NULL CONSTRAINT DF_Notes_Year DEFAULT 2026;
                    IF COL_LENGTH(N'dbo.Notes', N'Semester') IS NULL ALTER TABLE dbo.Notes ADD Semester int NOT NULL CONSTRAINT DF_Notes_Semester DEFAULT 0;
                    IF COL_LENGTH(N'dbo.Notes', N'OriginalFileName') IS NULL ALTER TABLE dbo.Notes ADD OriginalFileName nvarchar(255) NOT NULL CONSTRAINT DF_Notes_OriginalFileName DEFAULT N'';
                    IF COL_LENGTH(N'dbo.Notes', N'FileType') IS NULL ALTER TABLE dbo.Notes ADD FileType nvarchar(100) NULL;
                    IF COL_LENGTH(N'dbo.Notes', N'FileSizeBytes') IS NULL ALTER TABLE dbo.Notes ADD FileSizeBytes bigint NOT NULL CONSTRAINT DF_Notes_FileSizeBytes DEFAULT 0;
                    IF COL_LENGTH(N'dbo.Notes', N'UploadedByUserId') IS NULL ALTER TABLE dbo.Notes ADD UploadedByUserId int NOT NULL CONSTRAINT DF_Notes_UploadedByUserId DEFAULT 0;
                    IF COL_LENGTH(N'dbo.Notes', N'Status') IS NULL ALTER TABLE dbo.Notes ADD Status int NOT NULL CONSTRAINT DF_Notes_Status DEFAULT 0;
                    IF COL_LENGTH(N'dbo.Notes', N'PageCount') IS NULL ALTER TABLE dbo.Notes ADD PageCount int NOT NULL CONSTRAINT DF_Notes_PageCount DEFAULT 0;
                    IF COL_LENGTH(N'dbo.Notes', N'DownloadCount') IS NULL ALTER TABLE dbo.Notes ADD DownloadCount int NOT NULL CONSTRAINT DF_Notes_DownloadCount DEFAULT 0;
                    IF COL_LENGTH(N'dbo.Notes', N'ViewCount') IS NULL ALTER TABLE dbo.Notes ADD ViewCount int NOT NULL CONSTRAINT DF_Notes_ViewCount DEFAULT 0;
                    IF COL_LENGTH(N'dbo.Notes', N'AverageRating') IS NULL ALTER TABLE dbo.Notes ADD AverageRating decimal(3,2) NOT NULL CONSTRAINT DF_Notes_AverageRating DEFAULT 0;
                    IF COL_LENGTH(N'dbo.Notes', N'TotalRatings') IS NULL ALTER TABLE dbo.Notes ADD TotalRatings int NOT NULL CONSTRAINT DF_Notes_TotalRatings DEFAULT 0;
                    IF COL_LENGTH(N'dbo.Notes', N'CreatedDate') IS NULL ALTER TABLE dbo.Notes ADD CreatedDate datetime2 NOT NULL CONSTRAINT DF_Notes_CreatedDate DEFAULT SYSUTCDATETIME();
                    IF COL_LENGTH(N'dbo.Notes', N'ModifiedDate') IS NULL ALTER TABLE dbo.Notes ADD ModifiedDate datetime2 NULL;
                    IF COL_LENGTH(N'dbo.Notes', N'ApprovedDate') IS NULL ALTER TABLE dbo.Notes ADD ApprovedDate datetime2 NULL;
                    IF COL_LENGTH(N'dbo.Notes', N'ApprovalNotes') IS NULL ALTER TABLE dbo.Notes ADD ApprovalNotes nvarchar(500) NULL;
                    IF COL_LENGTH(N'dbo.Notes', N'Course') IS NOT NULL EXEC(N'UPDATE dbo.Notes SET CourseCode = LEFT(COALESCE(NULLIF(CourseCode, N''''), Course), 50)');
                    IF COL_LENGTH(N'dbo.Notes', N'FileName') IS NOT NULL EXEC(N'UPDATE dbo.Notes SET OriginalFileName = LEFT(COALESCE(NULLIF(OriginalFileName, N''''), FileName), 255)');
                    IF COL_LENGTH(N'dbo.Notes', N'UploadedAt') IS NOT NULL EXEC(N'UPDATE dbo.Notes SET CreatedDate = COALESCE(TRY_CONVERT(datetime2, UploadedAt), CreatedDate)');
                    EXEC(N'UPDATE dbo.Notes SET SubjectName = LEFT(COALESCE(NULLIF(SubjectName, N''''), Title, CourseCode), 200)');
                    IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name=N'IX_Notes_AverageRating' AND object_id=OBJECT_ID(N'dbo.Notes')) EXEC(N'CREATE INDEX IX_Notes_AverageRating ON dbo.Notes(AverageRating)');
                    IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name=N'IX_Notes_CreatedDate' AND object_id=OBJECT_ID(N'dbo.Notes')) EXEC(N'CREATE INDEX IX_Notes_CreatedDate ON dbo.Notes(CreatedDate)');
                    IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name=N'IX_Notes_Department_Year_Semester' AND object_id=OBJECT_ID(N'dbo.Notes')) EXEC(N'CREATE INDEX IX_Notes_Department_Year_Semester ON dbo.Notes(Department, Year, Semester)');
                    IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name=N'IX_Notes_Status' AND object_id=OBJECT_ID(N'dbo.Notes')) EXEC(N'CREATE INDEX IX_Notes_Status ON dbo.Notes(Status)');
                    IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name=N'IX_Notes_UploadedByUserId' AND object_id=OBJECT_ID(N'dbo.Notes')) EXEC(N'CREATE INDEX IX_Notes_UploadedByUserId ON dbo.Notes(UploadedByUserId)');
                END
                """);

            migrationBuilder.CreateTable(
                name: "Lectures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SubjectName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CourseCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ProfessorName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Department = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Semester = table.Column<int>(type: "int", nullable: false),
                    Topics = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FilePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    OriginalFileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FileType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FileSizeBytes = table.Column<long>(type: "bigint", nullable: false),
                    UploadedByUserId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    DownloadCount = table.Column<int>(type: "int", nullable: false),
                    ViewCount = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime?>(type: "datetime2", nullable: true),
                    ApprovedDate = table.Column<DateTime?>(type: "datetime2", nullable: true),
                    ApprovalNotes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lectures", x => x.Id);
                    table.ForeignKey("FK_Lectures_Users_UploadedByUserId", x => x.UploadedByUserId, "Users", "Id", onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex("IX_Lectures_CreatedDate", "Lectures", "CreatedDate");
            migrationBuilder.CreateIndex("IX_Lectures_Department_Year_Semester", "Lectures", new[] { "Department", "Year", "Semester" });
            migrationBuilder.CreateIndex("IX_Lectures_Status", "Lectures", "Status");
            migrationBuilder.CreateIndex("IX_Lectures_UploadedByUserId", "Lectures", "UploadedByUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "Lectures");
        }
    }
}
