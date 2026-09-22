using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace student_resource_hub.Migrations
{
    /// <inheritdoc />
    public partial class NormalizeAcademicSemesters : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE AcademicSemesters SET IsActive = 0 WHERE IsActive = 1 AND SortOrder > 8 AND AcademicYear IS NULL;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE AcademicSemesters SET IsActive = 1 WHERE IsActive = 0 AND SortOrder > 8 AND AcademicYear IS NULL;");
        }
    }
}
