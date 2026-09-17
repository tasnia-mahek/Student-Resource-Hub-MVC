using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace student_resource_hub.Migrations
{
    /// <inheritdoc />
    public partial class NormalizeNotesSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                IF COL_LENGTH(N'dbo.Notes', N'Course') IS NOT NULL
                    UPDATE dbo.Notes
                    SET CourseCode = LEFT(Course, 50)
                    WHERE NULLIF(CourseCode, N'') IS NULL AND NULLIF(Course, N'') IS NOT NULL;

                IF COL_LENGTH(N'dbo.Notes', N'FileName') IS NOT NULL
                    UPDATE dbo.Notes
                    SET OriginalFileName = LEFT(FileName, 255)
                    WHERE NULLIF(OriginalFileName, N'') IS NULL AND NULLIF(FileName, N'') IS NOT NULL;

                IF COL_LENGTH(N'dbo.Notes', N'UploadedAt') IS NOT NULL
                    UPDATE dbo.Notes
                    SET CreatedDate = UploadedAt
                    WHERE CreatedDate IS NULL AND UploadedAt IS NOT NULL;

                IF COL_LENGTH(N'dbo.Notes', N'Course') IS NOT NULL
                    ALTER TABLE dbo.Notes DROP COLUMN Course;

                IF COL_LENGTH(N'dbo.Notes', N'FileName') IS NOT NULL
                    ALTER TABLE dbo.Notes DROP COLUMN FileName;

                IF COL_LENGTH(N'dbo.Notes', N'UploadedBy') IS NOT NULL
                    ALTER TABLE dbo.Notes DROP COLUMN UploadedBy;

                IF COL_LENGTH(N'dbo.Notes', N'UploadedAt') IS NOT NULL
                    ALTER TABLE dbo.Notes DROP COLUMN UploadedAt;
                """);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Notes",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                IF COL_LENGTH(N'dbo.Notes', N'Course') IS NULL
                    ALTER TABLE dbo.Notes ADD Course nvarchar(100) NOT NULL CONSTRAINT DF_Notes_Course_Rollback DEFAULT N'';

                IF COL_LENGTH(N'dbo.Notes', N'FileName') IS NULL
                    ALTER TABLE dbo.Notes ADD FileName nvarchar(255) NOT NULL CONSTRAINT DF_Notes_FileName_Rollback DEFAULT N'';

                IF COL_LENGTH(N'dbo.Notes', N'UploadedBy') IS NULL
                    ALTER TABLE dbo.Notes ADD UploadedBy nvarchar(255) NOT NULL CONSTRAINT DF_Notes_UploadedBy_Rollback DEFAULT N'';

                IF COL_LENGTH(N'dbo.Notes', N'UploadedAt') IS NULL
                    ALTER TABLE dbo.Notes ADD UploadedAt datetime2 NOT NULL CONSTRAINT DF_Notes_UploadedAt_Rollback DEFAULT SYSUTCDATETIME();

                UPDATE dbo.Notes
                SET Course = LEFT(CourseCode, 100),
                    FileName = OriginalFileName,
                    UploadedAt = CreatedDate;

                UPDATE dbo.Notes SET Description = N'' WHERE Description IS NULL;
                """);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Notes",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);
        }
    }
}
