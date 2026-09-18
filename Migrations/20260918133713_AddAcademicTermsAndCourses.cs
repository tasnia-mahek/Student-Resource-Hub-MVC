using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace student_resource_hub.Migrations
{
    /// <inheritdoc />
    public partial class AddAcademicTermsAndCourses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AcademicCourseId",
                table: "PastPapers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AcademicCourseId",
                table: "Notes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AcademicCourseId",
                table: "Lectures",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AcademicYear",
                table: "AcademicSemesters",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TermName",
                table: "AcademicSemesters",
                type: "varchar(30)",
                maxLength: 30,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AcademicCourses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AcademicSemesterId = table.Column<int>(type: "int", nullable: false),
                    CourseCode = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsLab = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcademicCourses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AcademicCourses_AcademicSemesters_AcademicSemesterId",
                        column: x => x.AcademicSemesterId,
                        principalTable: "AcademicSemesters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "AcademicYear", "TermName" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "AcademicYear", "TermName" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "AcademicYear", "TermName" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "AcademicYear", "TermName" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "AcademicYear", "TermName" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "AcademicYear", "TermName" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "AcademicYear", "TermName" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "AcademicYear", "TermName" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "AcademicYear", "TermName" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "AcademicYear", "TermName" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "AcademicYear", "TermName" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "AcademicYear", "TermName" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 25,
                columns: new[] { "AcademicYear", "TermName" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 26,
                columns: new[] { "AcademicYear", "TermName" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 27,
                columns: new[] { "AcademicYear", "TermName" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 28,
                columns: new[] { "AcademicYear", "TermName" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 31,
                columns: new[] { "AcademicYear", "TermName" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 32,
                columns: new[] { "AcademicYear", "TermName" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 33,
                columns: new[] { "AcademicYear", "TermName" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 34,
                columns: new[] { "AcademicYear", "TermName" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 35,
                columns: new[] { "AcademicYear", "TermName" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 36,
                columns: new[] { "AcademicYear", "TermName" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 37,
                columns: new[] { "AcademicYear", "TermName" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 38,
                columns: new[] { "AcademicYear", "TermName" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 41,
                columns: new[] { "AcademicYear", "TermName" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 42,
                columns: new[] { "AcademicYear", "TermName" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 43,
                columns: new[] { "AcademicYear", "TermName" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 44,
                columns: new[] { "AcademicYear", "TermName" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 45,
                columns: new[] { "AcademicYear", "TermName" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 46,
                columns: new[] { "AcademicYear", "TermName" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 47,
                columns: new[] { "AcademicYear", "TermName" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 48,
                columns: new[] { "AcademicYear", "TermName" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 51,
                columns: new[] { "AcademicYear", "TermName" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 52,
                columns: new[] { "AcademicYear", "TermName" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 53,
                columns: new[] { "AcademicYear", "TermName" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 54,
                columns: new[] { "AcademicYear", "TermName" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 55,
                columns: new[] { "AcademicYear", "TermName" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 56,
                columns: new[] { "AcademicYear", "TermName" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 57,
                columns: new[] { "AcademicYear", "TermName" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 58,
                columns: new[] { "AcademicYear", "TermName" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 61,
                columns: new[] { "AcademicYear", "TermName" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 62,
                columns: new[] { "AcademicYear", "TermName" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 63,
                columns: new[] { "AcademicYear", "TermName" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 64,
                columns: new[] { "AcademicYear", "TermName" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 65,
                columns: new[] { "AcademicYear", "TermName" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 66,
                columns: new[] { "AcademicYear", "TermName" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 67,
                columns: new[] { "AcademicYear", "TermName" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 68,
                columns: new[] { "AcademicYear", "TermName" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 71,
                columns: new[] { "AcademicYear", "TermName" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 72,
                columns: new[] { "AcademicYear", "TermName" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 73,
                columns: new[] { "AcademicYear", "TermName" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 74,
                columns: new[] { "AcademicYear", "TermName" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 75,
                columns: new[] { "AcademicYear", "TermName" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 76,
                columns: new[] { "AcademicYear", "TermName" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 77,
                columns: new[] { "AcademicYear", "TermName" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 78,
                columns: new[] { "AcademicYear", "TermName" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 81,
                columns: new[] { "AcademicYear", "TermName" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 82,
                columns: new[] { "AcademicYear", "TermName" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 83,
                columns: new[] { "AcademicYear", "TermName" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 84,
                columns: new[] { "AcademicYear", "TermName" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 85,
                columns: new[] { "AcademicYear", "TermName" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 86,
                columns: new[] { "AcademicYear", "TermName" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 87,
                columns: new[] { "AcademicYear", "TermName" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 88,
                columns: new[] { "AcademicYear", "TermName" },
                values: new object[] { null, null });

            migrationBuilder.InsertData(
                table: "AcademicSemesters",
                columns: new[] { "Id", "AcademicYear", "DepartmentId", "IsActive", "Name", "SortOrder", "TermName" },
                values: new object[,]
                {
                    { 1000, 2024, 1, true, "Fall 2024", 20240, "Fall" },
                    { 1001, 2024, 1, true, "Spring 2024", 20241, "Spring" },
                    { 1002, 2024, 1, true, "Winter 2024", 20242, "Winter" },
                    { 1003, 2025, 1, true, "Fall 2025", 20250, "Fall" },
                    { 1004, 2025, 1, true, "Spring 2025", 20251, "Spring" },
                    { 1005, 2025, 1, true, "Winter 2025", 20252, "Winter" },
                    { 1006, 2026, 1, true, "Fall 2026", 20260, "Fall" },
                    { 1007, 2026, 1, true, "Spring 2026", 20261, "Spring" },
                    { 1008, 2026, 1, true, "Winter 2026", 20262, "Winter" },
                    { 1009, 2024, 2, true, "Fall 2024", 20240, "Fall" },
                    { 1010, 2024, 2, true, "Spring 2024", 20241, "Spring" },
                    { 1011, 2024, 2, true, "Winter 2024", 20242, "Winter" },
                    { 1012, 2025, 2, true, "Fall 2025", 20250, "Fall" },
                    { 1013, 2025, 2, true, "Spring 2025", 20251, "Spring" },
                    { 1014, 2025, 2, true, "Winter 2025", 20252, "Winter" },
                    { 1015, 2026, 2, true, "Fall 2026", 20260, "Fall" },
                    { 1016, 2026, 2, true, "Spring 2026", 20261, "Spring" },
                    { 1017, 2026, 2, true, "Winter 2026", 20262, "Winter" },
                    { 1018, 2024, 3, true, "Fall 2024", 20240, "Fall" },
                    { 1019, 2024, 3, true, "Spring 2024", 20241, "Spring" },
                    { 1020, 2024, 3, true, "Winter 2024", 20242, "Winter" },
                    { 1021, 2025, 3, true, "Fall 2025", 20250, "Fall" },
                    { 1022, 2025, 3, true, "Spring 2025", 20251, "Spring" },
                    { 1023, 2025, 3, true, "Winter 2025", 20252, "Winter" },
                    { 1024, 2026, 3, true, "Fall 2026", 20260, "Fall" },
                    { 1025, 2026, 3, true, "Spring 2026", 20261, "Spring" },
                    { 1026, 2026, 3, true, "Winter 2026", 20262, "Winter" },
                    { 1027, 2024, 4, true, "Fall 2024", 20240, "Fall" },
                    { 1028, 2024, 4, true, "Spring 2024", 20241, "Spring" },
                    { 1029, 2024, 4, true, "Winter 2024", 20242, "Winter" },
                    { 1030, 2025, 4, true, "Fall 2025", 20250, "Fall" },
                    { 1031, 2025, 4, true, "Spring 2025", 20251, "Spring" },
                    { 1032, 2025, 4, true, "Winter 2025", 20252, "Winter" },
                    { 1033, 2026, 4, true, "Fall 2026", 20260, "Fall" },
                    { 1034, 2026, 4, true, "Spring 2026", 20261, "Spring" },
                    { 1035, 2026, 4, true, "Winter 2026", 20262, "Winter" },
                    { 1036, 2024, 5, true, "Fall 2024", 20240, "Fall" },
                    { 1037, 2024, 5, true, "Spring 2024", 20241, "Spring" },
                    { 1038, 2024, 5, true, "Winter 2024", 20242, "Winter" },
                    { 1039, 2025, 5, true, "Fall 2025", 20250, "Fall" },
                    { 1040, 2025, 5, true, "Spring 2025", 20251, "Spring" },
                    { 1041, 2025, 5, true, "Winter 2025", 20252, "Winter" },
                    { 1042, 2026, 5, true, "Fall 2026", 20260, "Fall" },
                    { 1043, 2026, 5, true, "Spring 2026", 20261, "Spring" },
                    { 1044, 2026, 5, true, "Winter 2026", 20262, "Winter" },
                    { 1045, 2024, 6, true, "Fall 2024", 20240, "Fall" },
                    { 1046, 2024, 6, true, "Spring 2024", 20241, "Spring" },
                    { 1047, 2024, 6, true, "Winter 2024", 20242, "Winter" },
                    { 1048, 2025, 6, true, "Fall 2025", 20250, "Fall" },
                    { 1049, 2025, 6, true, "Spring 2025", 20251, "Spring" },
                    { 1050, 2025, 6, true, "Winter 2025", 20252, "Winter" },
                    { 1051, 2026, 6, true, "Fall 2026", 20260, "Fall" },
                    { 1052, 2026, 6, true, "Spring 2026", 20261, "Spring" },
                    { 1053, 2026, 6, true, "Winter 2026", 20262, "Winter" },
                    { 1054, 2024, 7, true, "Fall 2024", 20240, "Fall" },
                    { 1055, 2024, 7, true, "Spring 2024", 20241, "Spring" },
                    { 1056, 2024, 7, true, "Winter 2024", 20242, "Winter" },
                    { 1057, 2025, 7, true, "Fall 2025", 20250, "Fall" },
                    { 1058, 2025, 7, true, "Spring 2025", 20251, "Spring" },
                    { 1059, 2025, 7, true, "Winter 2025", 20252, "Winter" },
                    { 1060, 2026, 7, true, "Fall 2026", 20260, "Fall" },
                    { 1061, 2026, 7, true, "Spring 2026", 20261, "Spring" },
                    { 1062, 2026, 7, true, "Winter 2026", 20262, "Winter" },
                    { 1063, 2024, 8, true, "Fall 2024", 20240, "Fall" },
                    { 1064, 2024, 8, true, "Spring 2024", 20241, "Spring" },
                    { 1065, 2024, 8, true, "Winter 2024", 20242, "Winter" },
                    { 1066, 2025, 8, true, "Fall 2025", 20250, "Fall" },
                    { 1067, 2025, 8, true, "Spring 2025", 20251, "Spring" },
                    { 1068, 2025, 8, true, "Winter 2025", 20252, "Winter" },
                    { 1069, 2026, 8, true, "Fall 2026", 20260, "Fall" },
                    { 1070, 2026, 8, true, "Spring 2026", 20261, "Spring" },
                    { 1071, 2026, 8, true, "Winter 2026", 20262, "Winter" }
                });

            migrationBuilder.InsertData(
                table: "AcademicCourses",
                columns: new[] { "Id", "AcademicSemesterId", "CourseCode", "IsActive", "IsLab", "Name" },
                values: new object[,]
                {
                    { 10000, 1000, "1C101", true, false, "Programming Fundamentals" },
                    { 10001, 1000, "1C102", true, false, "Discrete Mathematics" },
                    { 10002, 1000, "1C103", true, false, "Data Structures" },
                    { 10003, 1000, "1C104", true, false, "Digital Logic" },
                    { 10004, 1000, "1C105", true, false, "Communication Skills" },
                    { 10005, 1000, "1L101", true, true, "Programming Lab" },
                    { 10006, 1000, "1L102", true, true, "Digital Systems Lab" },
                    { 10007, 1000, "1L103", true, true, "Engineering Drawing Lab" },
                    { 10008, 1000, "1L104", true, true, "Project and Design Lab" },
                    { 10009, 1001, "1C101", true, false, "Programming Fundamentals" },
                    { 10010, 1001, "1C102", true, false, "Discrete Mathematics" },
                    { 10011, 1001, "1C103", true, false, "Data Structures" },
                    { 10012, 1001, "1C104", true, false, "Digital Logic" },
                    { 10013, 1001, "1C105", true, false, "Communication Skills" },
                    { 10014, 1001, "1L101", true, true, "Programming Lab" },
                    { 10015, 1001, "1L102", true, true, "Digital Systems Lab" },
                    { 10016, 1001, "1L103", true, true, "Engineering Drawing Lab" },
                    { 10017, 1001, "1L104", true, true, "Project and Design Lab" },
                    { 10018, 1002, "1C101", true, false, "Programming Fundamentals" },
                    { 10019, 1002, "1C102", true, false, "Discrete Mathematics" },
                    { 10020, 1002, "1C103", true, false, "Data Structures" },
                    { 10021, 1002, "1C104", true, false, "Digital Logic" },
                    { 10022, 1002, "1C105", true, false, "Communication Skills" },
                    { 10023, 1002, "1L101", true, true, "Programming Lab" },
                    { 10024, 1002, "1L102", true, true, "Digital Systems Lab" },
                    { 10025, 1002, "1L103", true, true, "Engineering Drawing Lab" },
                    { 10026, 1002, "1L104", true, true, "Project and Design Lab" },
                    { 10027, 1003, "1C101", true, false, "Programming Fundamentals" },
                    { 10028, 1003, "1C102", true, false, "Discrete Mathematics" },
                    { 10029, 1003, "1C103", true, false, "Data Structures" },
                    { 10030, 1003, "1C104", true, false, "Digital Logic" },
                    { 10031, 1003, "1C105", true, false, "Communication Skills" },
                    { 10032, 1003, "1L101", true, true, "Programming Lab" },
                    { 10033, 1003, "1L102", true, true, "Digital Systems Lab" },
                    { 10034, 1003, "1L103", true, true, "Engineering Drawing Lab" },
                    { 10035, 1003, "1L104", true, true, "Project and Design Lab" },
                    { 10036, 1004, "1C101", true, false, "Programming Fundamentals" },
                    { 10037, 1004, "1C102", true, false, "Discrete Mathematics" },
                    { 10038, 1004, "1C103", true, false, "Data Structures" },
                    { 10039, 1004, "1C104", true, false, "Digital Logic" },
                    { 10040, 1004, "1C105", true, false, "Communication Skills" },
                    { 10041, 1004, "1L101", true, true, "Programming Lab" },
                    { 10042, 1004, "1L102", true, true, "Digital Systems Lab" },
                    { 10043, 1004, "1L103", true, true, "Engineering Drawing Lab" },
                    { 10044, 1004, "1L104", true, true, "Project and Design Lab" },
                    { 10045, 1005, "1C101", true, false, "Programming Fundamentals" },
                    { 10046, 1005, "1C102", true, false, "Discrete Mathematics" },
                    { 10047, 1005, "1C103", true, false, "Data Structures" },
                    { 10048, 1005, "1C104", true, false, "Digital Logic" },
                    { 10049, 1005, "1C105", true, false, "Communication Skills" },
                    { 10050, 1005, "1L101", true, true, "Programming Lab" },
                    { 10051, 1005, "1L102", true, true, "Digital Systems Lab" },
                    { 10052, 1005, "1L103", true, true, "Engineering Drawing Lab" },
                    { 10053, 1005, "1L104", true, true, "Project and Design Lab" },
                    { 10054, 1006, "1C101", true, false, "Programming Fundamentals" },
                    { 10055, 1006, "1C102", true, false, "Discrete Mathematics" },
                    { 10056, 1006, "1C103", true, false, "Data Structures" },
                    { 10057, 1006, "1C104", true, false, "Digital Logic" },
                    { 10058, 1006, "1C105", true, false, "Communication Skills" },
                    { 10059, 1006, "1L101", true, true, "Programming Lab" },
                    { 10060, 1006, "1L102", true, true, "Digital Systems Lab" },
                    { 10061, 1006, "1L103", true, true, "Engineering Drawing Lab" },
                    { 10062, 1006, "1L104", true, true, "Project and Design Lab" },
                    { 10063, 1007, "1C101", true, false, "Programming Fundamentals" },
                    { 10064, 1007, "1C102", true, false, "Discrete Mathematics" },
                    { 10065, 1007, "1C103", true, false, "Data Structures" },
                    { 10066, 1007, "1C104", true, false, "Digital Logic" },
                    { 10067, 1007, "1C105", true, false, "Communication Skills" },
                    { 10068, 1007, "1L101", true, true, "Programming Lab" },
                    { 10069, 1007, "1L102", true, true, "Digital Systems Lab" },
                    { 10070, 1007, "1L103", true, true, "Engineering Drawing Lab" },
                    { 10071, 1007, "1L104", true, true, "Project and Design Lab" },
                    { 10072, 1008, "1C101", true, false, "Programming Fundamentals" },
                    { 10073, 1008, "1C102", true, false, "Discrete Mathematics" },
                    { 10074, 1008, "1C103", true, false, "Data Structures" },
                    { 10075, 1008, "1C104", true, false, "Digital Logic" },
                    { 10076, 1008, "1C105", true, false, "Communication Skills" },
                    { 10077, 1008, "1L101", true, true, "Programming Lab" },
                    { 10078, 1008, "1L102", true, true, "Digital Systems Lab" },
                    { 10079, 1008, "1L103", true, true, "Engineering Drawing Lab" },
                    { 10080, 1008, "1L104", true, true, "Project and Design Lab" },
                    { 10081, 1009, "2C101", true, false, "Programming Fundamentals" },
                    { 10082, 1009, "2C102", true, false, "Discrete Mathematics" },
                    { 10083, 1009, "2C103", true, false, "Data Structures" },
                    { 10084, 1009, "2C104", true, false, "Digital Logic" },
                    { 10085, 1009, "2C105", true, false, "Communication Skills" },
                    { 10086, 1009, "2L101", true, true, "Programming Lab" },
                    { 10087, 1009, "2L102", true, true, "Digital Systems Lab" },
                    { 10088, 1009, "2L103", true, true, "Engineering Drawing Lab" },
                    { 10089, 1009, "2L104", true, true, "Project and Design Lab" },
                    { 10090, 1010, "2C101", true, false, "Programming Fundamentals" },
                    { 10091, 1010, "2C102", true, false, "Discrete Mathematics" },
                    { 10092, 1010, "2C103", true, false, "Data Structures" },
                    { 10093, 1010, "2C104", true, false, "Digital Logic" },
                    { 10094, 1010, "2C105", true, false, "Communication Skills" },
                    { 10095, 1010, "2L101", true, true, "Programming Lab" },
                    { 10096, 1010, "2L102", true, true, "Digital Systems Lab" },
                    { 10097, 1010, "2L103", true, true, "Engineering Drawing Lab" },
                    { 10098, 1010, "2L104", true, true, "Project and Design Lab" },
                    { 10099, 1011, "2C101", true, false, "Programming Fundamentals" },
                    { 10100, 1011, "2C102", true, false, "Discrete Mathematics" },
                    { 10101, 1011, "2C103", true, false, "Data Structures" },
                    { 10102, 1011, "2C104", true, false, "Digital Logic" },
                    { 10103, 1011, "2C105", true, false, "Communication Skills" },
                    { 10104, 1011, "2L101", true, true, "Programming Lab" },
                    { 10105, 1011, "2L102", true, true, "Digital Systems Lab" },
                    { 10106, 1011, "2L103", true, true, "Engineering Drawing Lab" },
                    { 10107, 1011, "2L104", true, true, "Project and Design Lab" },
                    { 10108, 1012, "2C101", true, false, "Programming Fundamentals" },
                    { 10109, 1012, "2C102", true, false, "Discrete Mathematics" },
                    { 10110, 1012, "2C103", true, false, "Data Structures" },
                    { 10111, 1012, "2C104", true, false, "Digital Logic" },
                    { 10112, 1012, "2C105", true, false, "Communication Skills" },
                    { 10113, 1012, "2L101", true, true, "Programming Lab" },
                    { 10114, 1012, "2L102", true, true, "Digital Systems Lab" },
                    { 10115, 1012, "2L103", true, true, "Engineering Drawing Lab" },
                    { 10116, 1012, "2L104", true, true, "Project and Design Lab" },
                    { 10117, 1013, "2C101", true, false, "Programming Fundamentals" },
                    { 10118, 1013, "2C102", true, false, "Discrete Mathematics" },
                    { 10119, 1013, "2C103", true, false, "Data Structures" },
                    { 10120, 1013, "2C104", true, false, "Digital Logic" },
                    { 10121, 1013, "2C105", true, false, "Communication Skills" },
                    { 10122, 1013, "2L101", true, true, "Programming Lab" },
                    { 10123, 1013, "2L102", true, true, "Digital Systems Lab" },
                    { 10124, 1013, "2L103", true, true, "Engineering Drawing Lab" },
                    { 10125, 1013, "2L104", true, true, "Project and Design Lab" },
                    { 10126, 1014, "2C101", true, false, "Programming Fundamentals" },
                    { 10127, 1014, "2C102", true, false, "Discrete Mathematics" },
                    { 10128, 1014, "2C103", true, false, "Data Structures" },
                    { 10129, 1014, "2C104", true, false, "Digital Logic" },
                    { 10130, 1014, "2C105", true, false, "Communication Skills" },
                    { 10131, 1014, "2L101", true, true, "Programming Lab" },
                    { 10132, 1014, "2L102", true, true, "Digital Systems Lab" },
                    { 10133, 1014, "2L103", true, true, "Engineering Drawing Lab" },
                    { 10134, 1014, "2L104", true, true, "Project and Design Lab" },
                    { 10135, 1015, "2C101", true, false, "Programming Fundamentals" },
                    { 10136, 1015, "2C102", true, false, "Discrete Mathematics" },
                    { 10137, 1015, "2C103", true, false, "Data Structures" },
                    { 10138, 1015, "2C104", true, false, "Digital Logic" },
                    { 10139, 1015, "2C105", true, false, "Communication Skills" },
                    { 10140, 1015, "2L101", true, true, "Programming Lab" },
                    { 10141, 1015, "2L102", true, true, "Digital Systems Lab" },
                    { 10142, 1015, "2L103", true, true, "Engineering Drawing Lab" },
                    { 10143, 1015, "2L104", true, true, "Project and Design Lab" },
                    { 10144, 1016, "2C101", true, false, "Programming Fundamentals" },
                    { 10145, 1016, "2C102", true, false, "Discrete Mathematics" },
                    { 10146, 1016, "2C103", true, false, "Data Structures" },
                    { 10147, 1016, "2C104", true, false, "Digital Logic" },
                    { 10148, 1016, "2C105", true, false, "Communication Skills" },
                    { 10149, 1016, "2L101", true, true, "Programming Lab" },
                    { 10150, 1016, "2L102", true, true, "Digital Systems Lab" },
                    { 10151, 1016, "2L103", true, true, "Engineering Drawing Lab" },
                    { 10152, 1016, "2L104", true, true, "Project and Design Lab" },
                    { 10153, 1017, "2C101", true, false, "Programming Fundamentals" },
                    { 10154, 1017, "2C102", true, false, "Discrete Mathematics" },
                    { 10155, 1017, "2C103", true, false, "Data Structures" },
                    { 10156, 1017, "2C104", true, false, "Digital Logic" },
                    { 10157, 1017, "2C105", true, false, "Communication Skills" },
                    { 10158, 1017, "2L101", true, true, "Programming Lab" },
                    { 10159, 1017, "2L102", true, true, "Digital Systems Lab" },
                    { 10160, 1017, "2L103", true, true, "Engineering Drawing Lab" },
                    { 10161, 1017, "2L104", true, true, "Project and Design Lab" },
                    { 10162, 1018, "3C101", true, false, "Programming Fundamentals" },
                    { 10163, 1018, "3C102", true, false, "Discrete Mathematics" },
                    { 10164, 1018, "3C103", true, false, "Data Structures" },
                    { 10165, 1018, "3C104", true, false, "Digital Logic" },
                    { 10166, 1018, "3C105", true, false, "Communication Skills" },
                    { 10167, 1018, "3L101", true, true, "Programming Lab" },
                    { 10168, 1018, "3L102", true, true, "Digital Systems Lab" },
                    { 10169, 1018, "3L103", true, true, "Engineering Drawing Lab" },
                    { 10170, 1018, "3L104", true, true, "Project and Design Lab" },
                    { 10171, 1019, "3C101", true, false, "Programming Fundamentals" },
                    { 10172, 1019, "3C102", true, false, "Discrete Mathematics" },
                    { 10173, 1019, "3C103", true, false, "Data Structures" },
                    { 10174, 1019, "3C104", true, false, "Digital Logic" },
                    { 10175, 1019, "3C105", true, false, "Communication Skills" },
                    { 10176, 1019, "3L101", true, true, "Programming Lab" },
                    { 10177, 1019, "3L102", true, true, "Digital Systems Lab" },
                    { 10178, 1019, "3L103", true, true, "Engineering Drawing Lab" },
                    { 10179, 1019, "3L104", true, true, "Project and Design Lab" },
                    { 10180, 1020, "3C101", true, false, "Programming Fundamentals" },
                    { 10181, 1020, "3C102", true, false, "Discrete Mathematics" },
                    { 10182, 1020, "3C103", true, false, "Data Structures" },
                    { 10183, 1020, "3C104", true, false, "Digital Logic" },
                    { 10184, 1020, "3C105", true, false, "Communication Skills" },
                    { 10185, 1020, "3L101", true, true, "Programming Lab" },
                    { 10186, 1020, "3L102", true, true, "Digital Systems Lab" },
                    { 10187, 1020, "3L103", true, true, "Engineering Drawing Lab" },
                    { 10188, 1020, "3L104", true, true, "Project and Design Lab" },
                    { 10189, 1021, "3C101", true, false, "Programming Fundamentals" },
                    { 10190, 1021, "3C102", true, false, "Discrete Mathematics" },
                    { 10191, 1021, "3C103", true, false, "Data Structures" },
                    { 10192, 1021, "3C104", true, false, "Digital Logic" },
                    { 10193, 1021, "3C105", true, false, "Communication Skills" },
                    { 10194, 1021, "3L101", true, true, "Programming Lab" },
                    { 10195, 1021, "3L102", true, true, "Digital Systems Lab" },
                    { 10196, 1021, "3L103", true, true, "Engineering Drawing Lab" },
                    { 10197, 1021, "3L104", true, true, "Project and Design Lab" },
                    { 10198, 1022, "3C101", true, false, "Programming Fundamentals" },
                    { 10199, 1022, "3C102", true, false, "Discrete Mathematics" },
                    { 10200, 1022, "3C103", true, false, "Data Structures" },
                    { 10201, 1022, "3C104", true, false, "Digital Logic" },
                    { 10202, 1022, "3C105", true, false, "Communication Skills" },
                    { 10203, 1022, "3L101", true, true, "Programming Lab" },
                    { 10204, 1022, "3L102", true, true, "Digital Systems Lab" },
                    { 10205, 1022, "3L103", true, true, "Engineering Drawing Lab" },
                    { 10206, 1022, "3L104", true, true, "Project and Design Lab" },
                    { 10207, 1023, "3C101", true, false, "Programming Fundamentals" },
                    { 10208, 1023, "3C102", true, false, "Discrete Mathematics" },
                    { 10209, 1023, "3C103", true, false, "Data Structures" },
                    { 10210, 1023, "3C104", true, false, "Digital Logic" },
                    { 10211, 1023, "3C105", true, false, "Communication Skills" },
                    { 10212, 1023, "3L101", true, true, "Programming Lab" },
                    { 10213, 1023, "3L102", true, true, "Digital Systems Lab" },
                    { 10214, 1023, "3L103", true, true, "Engineering Drawing Lab" },
                    { 10215, 1023, "3L104", true, true, "Project and Design Lab" },
                    { 10216, 1024, "3C101", true, false, "Programming Fundamentals" },
                    { 10217, 1024, "3C102", true, false, "Discrete Mathematics" },
                    { 10218, 1024, "3C103", true, false, "Data Structures" },
                    { 10219, 1024, "3C104", true, false, "Digital Logic" },
                    { 10220, 1024, "3C105", true, false, "Communication Skills" },
                    { 10221, 1024, "3L101", true, true, "Programming Lab" },
                    { 10222, 1024, "3L102", true, true, "Digital Systems Lab" },
                    { 10223, 1024, "3L103", true, true, "Engineering Drawing Lab" },
                    { 10224, 1024, "3L104", true, true, "Project and Design Lab" },
                    { 10225, 1025, "3C101", true, false, "Programming Fundamentals" },
                    { 10226, 1025, "3C102", true, false, "Discrete Mathematics" },
                    { 10227, 1025, "3C103", true, false, "Data Structures" },
                    { 10228, 1025, "3C104", true, false, "Digital Logic" },
                    { 10229, 1025, "3C105", true, false, "Communication Skills" },
                    { 10230, 1025, "3L101", true, true, "Programming Lab" },
                    { 10231, 1025, "3L102", true, true, "Digital Systems Lab" },
                    { 10232, 1025, "3L103", true, true, "Engineering Drawing Lab" },
                    { 10233, 1025, "3L104", true, true, "Project and Design Lab" },
                    { 10234, 1026, "3C101", true, false, "Programming Fundamentals" },
                    { 10235, 1026, "3C102", true, false, "Discrete Mathematics" },
                    { 10236, 1026, "3C103", true, false, "Data Structures" },
                    { 10237, 1026, "3C104", true, false, "Digital Logic" },
                    { 10238, 1026, "3C105", true, false, "Communication Skills" },
                    { 10239, 1026, "3L101", true, true, "Programming Lab" },
                    { 10240, 1026, "3L102", true, true, "Digital Systems Lab" },
                    { 10241, 1026, "3L103", true, true, "Engineering Drawing Lab" },
                    { 10242, 1026, "3L104", true, true, "Project and Design Lab" },
                    { 10243, 1027, "4C101", true, false, "Programming Fundamentals" },
                    { 10244, 1027, "4C102", true, false, "Discrete Mathematics" },
                    { 10245, 1027, "4C103", true, false, "Data Structures" },
                    { 10246, 1027, "4C104", true, false, "Digital Logic" },
                    { 10247, 1027, "4C105", true, false, "Communication Skills" },
                    { 10248, 1027, "4L101", true, true, "Programming Lab" },
                    { 10249, 1027, "4L102", true, true, "Digital Systems Lab" },
                    { 10250, 1027, "4L103", true, true, "Engineering Drawing Lab" },
                    { 10251, 1027, "4L104", true, true, "Project and Design Lab" },
                    { 10252, 1028, "4C101", true, false, "Programming Fundamentals" },
                    { 10253, 1028, "4C102", true, false, "Discrete Mathematics" },
                    { 10254, 1028, "4C103", true, false, "Data Structures" },
                    { 10255, 1028, "4C104", true, false, "Digital Logic" },
                    { 10256, 1028, "4C105", true, false, "Communication Skills" },
                    { 10257, 1028, "4L101", true, true, "Programming Lab" },
                    { 10258, 1028, "4L102", true, true, "Digital Systems Lab" },
                    { 10259, 1028, "4L103", true, true, "Engineering Drawing Lab" },
                    { 10260, 1028, "4L104", true, true, "Project and Design Lab" },
                    { 10261, 1029, "4C101", true, false, "Programming Fundamentals" },
                    { 10262, 1029, "4C102", true, false, "Discrete Mathematics" },
                    { 10263, 1029, "4C103", true, false, "Data Structures" },
                    { 10264, 1029, "4C104", true, false, "Digital Logic" },
                    { 10265, 1029, "4C105", true, false, "Communication Skills" },
                    { 10266, 1029, "4L101", true, true, "Programming Lab" },
                    { 10267, 1029, "4L102", true, true, "Digital Systems Lab" },
                    { 10268, 1029, "4L103", true, true, "Engineering Drawing Lab" },
                    { 10269, 1029, "4L104", true, true, "Project and Design Lab" },
                    { 10270, 1030, "4C101", true, false, "Programming Fundamentals" },
                    { 10271, 1030, "4C102", true, false, "Discrete Mathematics" },
                    { 10272, 1030, "4C103", true, false, "Data Structures" },
                    { 10273, 1030, "4C104", true, false, "Digital Logic" },
                    { 10274, 1030, "4C105", true, false, "Communication Skills" },
                    { 10275, 1030, "4L101", true, true, "Programming Lab" },
                    { 10276, 1030, "4L102", true, true, "Digital Systems Lab" },
                    { 10277, 1030, "4L103", true, true, "Engineering Drawing Lab" },
                    { 10278, 1030, "4L104", true, true, "Project and Design Lab" },
                    { 10279, 1031, "4C101", true, false, "Programming Fundamentals" },
                    { 10280, 1031, "4C102", true, false, "Discrete Mathematics" },
                    { 10281, 1031, "4C103", true, false, "Data Structures" },
                    { 10282, 1031, "4C104", true, false, "Digital Logic" },
                    { 10283, 1031, "4C105", true, false, "Communication Skills" },
                    { 10284, 1031, "4L101", true, true, "Programming Lab" },
                    { 10285, 1031, "4L102", true, true, "Digital Systems Lab" },
                    { 10286, 1031, "4L103", true, true, "Engineering Drawing Lab" },
                    { 10287, 1031, "4L104", true, true, "Project and Design Lab" },
                    { 10288, 1032, "4C101", true, false, "Programming Fundamentals" },
                    { 10289, 1032, "4C102", true, false, "Discrete Mathematics" },
                    { 10290, 1032, "4C103", true, false, "Data Structures" },
                    { 10291, 1032, "4C104", true, false, "Digital Logic" },
                    { 10292, 1032, "4C105", true, false, "Communication Skills" },
                    { 10293, 1032, "4L101", true, true, "Programming Lab" },
                    { 10294, 1032, "4L102", true, true, "Digital Systems Lab" },
                    { 10295, 1032, "4L103", true, true, "Engineering Drawing Lab" },
                    { 10296, 1032, "4L104", true, true, "Project and Design Lab" },
                    { 10297, 1033, "4C101", true, false, "Programming Fundamentals" },
                    { 10298, 1033, "4C102", true, false, "Discrete Mathematics" },
                    { 10299, 1033, "4C103", true, false, "Data Structures" },
                    { 10300, 1033, "4C104", true, false, "Digital Logic" },
                    { 10301, 1033, "4C105", true, false, "Communication Skills" },
                    { 10302, 1033, "4L101", true, true, "Programming Lab" },
                    { 10303, 1033, "4L102", true, true, "Digital Systems Lab" },
                    { 10304, 1033, "4L103", true, true, "Engineering Drawing Lab" },
                    { 10305, 1033, "4L104", true, true, "Project and Design Lab" },
                    { 10306, 1034, "4C101", true, false, "Programming Fundamentals" },
                    { 10307, 1034, "4C102", true, false, "Discrete Mathematics" },
                    { 10308, 1034, "4C103", true, false, "Data Structures" },
                    { 10309, 1034, "4C104", true, false, "Digital Logic" },
                    { 10310, 1034, "4C105", true, false, "Communication Skills" },
                    { 10311, 1034, "4L101", true, true, "Programming Lab" },
                    { 10312, 1034, "4L102", true, true, "Digital Systems Lab" },
                    { 10313, 1034, "4L103", true, true, "Engineering Drawing Lab" },
                    { 10314, 1034, "4L104", true, true, "Project and Design Lab" },
                    { 10315, 1035, "4C101", true, false, "Programming Fundamentals" },
                    { 10316, 1035, "4C102", true, false, "Discrete Mathematics" },
                    { 10317, 1035, "4C103", true, false, "Data Structures" },
                    { 10318, 1035, "4C104", true, false, "Digital Logic" },
                    { 10319, 1035, "4C105", true, false, "Communication Skills" },
                    { 10320, 1035, "4L101", true, true, "Programming Lab" },
                    { 10321, 1035, "4L102", true, true, "Digital Systems Lab" },
                    { 10322, 1035, "4L103", true, true, "Engineering Drawing Lab" },
                    { 10323, 1035, "4L104", true, true, "Project and Design Lab" },
                    { 10324, 1036, "5C101", true, false, "Programming Fundamentals" },
                    { 10325, 1036, "5C102", true, false, "Discrete Mathematics" },
                    { 10326, 1036, "5C103", true, false, "Data Structures" },
                    { 10327, 1036, "5C104", true, false, "Digital Logic" },
                    { 10328, 1036, "5C105", true, false, "Communication Skills" },
                    { 10329, 1036, "5L101", true, true, "Programming Lab" },
                    { 10330, 1036, "5L102", true, true, "Digital Systems Lab" },
                    { 10331, 1036, "5L103", true, true, "Engineering Drawing Lab" },
                    { 10332, 1036, "5L104", true, true, "Project and Design Lab" },
                    { 10333, 1037, "5C101", true, false, "Programming Fundamentals" },
                    { 10334, 1037, "5C102", true, false, "Discrete Mathematics" },
                    { 10335, 1037, "5C103", true, false, "Data Structures" },
                    { 10336, 1037, "5C104", true, false, "Digital Logic" },
                    { 10337, 1037, "5C105", true, false, "Communication Skills" },
                    { 10338, 1037, "5L101", true, true, "Programming Lab" },
                    { 10339, 1037, "5L102", true, true, "Digital Systems Lab" },
                    { 10340, 1037, "5L103", true, true, "Engineering Drawing Lab" },
                    { 10341, 1037, "5L104", true, true, "Project and Design Lab" },
                    { 10342, 1038, "5C101", true, false, "Programming Fundamentals" },
                    { 10343, 1038, "5C102", true, false, "Discrete Mathematics" },
                    { 10344, 1038, "5C103", true, false, "Data Structures" },
                    { 10345, 1038, "5C104", true, false, "Digital Logic" },
                    { 10346, 1038, "5C105", true, false, "Communication Skills" },
                    { 10347, 1038, "5L101", true, true, "Programming Lab" },
                    { 10348, 1038, "5L102", true, true, "Digital Systems Lab" },
                    { 10349, 1038, "5L103", true, true, "Engineering Drawing Lab" },
                    { 10350, 1038, "5L104", true, true, "Project and Design Lab" },
                    { 10351, 1039, "5C101", true, false, "Programming Fundamentals" },
                    { 10352, 1039, "5C102", true, false, "Discrete Mathematics" },
                    { 10353, 1039, "5C103", true, false, "Data Structures" },
                    { 10354, 1039, "5C104", true, false, "Digital Logic" },
                    { 10355, 1039, "5C105", true, false, "Communication Skills" },
                    { 10356, 1039, "5L101", true, true, "Programming Lab" },
                    { 10357, 1039, "5L102", true, true, "Digital Systems Lab" },
                    { 10358, 1039, "5L103", true, true, "Engineering Drawing Lab" },
                    { 10359, 1039, "5L104", true, true, "Project and Design Lab" },
                    { 10360, 1040, "5C101", true, false, "Programming Fundamentals" },
                    { 10361, 1040, "5C102", true, false, "Discrete Mathematics" },
                    { 10362, 1040, "5C103", true, false, "Data Structures" },
                    { 10363, 1040, "5C104", true, false, "Digital Logic" },
                    { 10364, 1040, "5C105", true, false, "Communication Skills" },
                    { 10365, 1040, "5L101", true, true, "Programming Lab" },
                    { 10366, 1040, "5L102", true, true, "Digital Systems Lab" },
                    { 10367, 1040, "5L103", true, true, "Engineering Drawing Lab" },
                    { 10368, 1040, "5L104", true, true, "Project and Design Lab" },
                    { 10369, 1041, "5C101", true, false, "Programming Fundamentals" },
                    { 10370, 1041, "5C102", true, false, "Discrete Mathematics" },
                    { 10371, 1041, "5C103", true, false, "Data Structures" },
                    { 10372, 1041, "5C104", true, false, "Digital Logic" },
                    { 10373, 1041, "5C105", true, false, "Communication Skills" },
                    { 10374, 1041, "5L101", true, true, "Programming Lab" },
                    { 10375, 1041, "5L102", true, true, "Digital Systems Lab" },
                    { 10376, 1041, "5L103", true, true, "Engineering Drawing Lab" },
                    { 10377, 1041, "5L104", true, true, "Project and Design Lab" },
                    { 10378, 1042, "5C101", true, false, "Programming Fundamentals" },
                    { 10379, 1042, "5C102", true, false, "Discrete Mathematics" },
                    { 10380, 1042, "5C103", true, false, "Data Structures" },
                    { 10381, 1042, "5C104", true, false, "Digital Logic" },
                    { 10382, 1042, "5C105", true, false, "Communication Skills" },
                    { 10383, 1042, "5L101", true, true, "Programming Lab" },
                    { 10384, 1042, "5L102", true, true, "Digital Systems Lab" },
                    { 10385, 1042, "5L103", true, true, "Engineering Drawing Lab" },
                    { 10386, 1042, "5L104", true, true, "Project and Design Lab" },
                    { 10387, 1043, "5C101", true, false, "Programming Fundamentals" },
                    { 10388, 1043, "5C102", true, false, "Discrete Mathematics" },
                    { 10389, 1043, "5C103", true, false, "Data Structures" },
                    { 10390, 1043, "5C104", true, false, "Digital Logic" },
                    { 10391, 1043, "5C105", true, false, "Communication Skills" },
                    { 10392, 1043, "5L101", true, true, "Programming Lab" },
                    { 10393, 1043, "5L102", true, true, "Digital Systems Lab" },
                    { 10394, 1043, "5L103", true, true, "Engineering Drawing Lab" },
                    { 10395, 1043, "5L104", true, true, "Project and Design Lab" },
                    { 10396, 1044, "5C101", true, false, "Programming Fundamentals" },
                    { 10397, 1044, "5C102", true, false, "Discrete Mathematics" },
                    { 10398, 1044, "5C103", true, false, "Data Structures" },
                    { 10399, 1044, "5C104", true, false, "Digital Logic" },
                    { 10400, 1044, "5C105", true, false, "Communication Skills" },
                    { 10401, 1044, "5L101", true, true, "Programming Lab" },
                    { 10402, 1044, "5L102", true, true, "Digital Systems Lab" },
                    { 10403, 1044, "5L103", true, true, "Engineering Drawing Lab" },
                    { 10404, 1044, "5L104", true, true, "Project and Design Lab" },
                    { 10405, 1045, "6C101", true, false, "Programming Fundamentals" },
                    { 10406, 1045, "6C102", true, false, "Discrete Mathematics" },
                    { 10407, 1045, "6C103", true, false, "Data Structures" },
                    { 10408, 1045, "6C104", true, false, "Digital Logic" },
                    { 10409, 1045, "6C105", true, false, "Communication Skills" },
                    { 10410, 1045, "6L101", true, true, "Programming Lab" },
                    { 10411, 1045, "6L102", true, true, "Digital Systems Lab" },
                    { 10412, 1045, "6L103", true, true, "Engineering Drawing Lab" },
                    { 10413, 1045, "6L104", true, true, "Project and Design Lab" },
                    { 10414, 1046, "6C101", true, false, "Programming Fundamentals" },
                    { 10415, 1046, "6C102", true, false, "Discrete Mathematics" },
                    { 10416, 1046, "6C103", true, false, "Data Structures" },
                    { 10417, 1046, "6C104", true, false, "Digital Logic" },
                    { 10418, 1046, "6C105", true, false, "Communication Skills" },
                    { 10419, 1046, "6L101", true, true, "Programming Lab" },
                    { 10420, 1046, "6L102", true, true, "Digital Systems Lab" },
                    { 10421, 1046, "6L103", true, true, "Engineering Drawing Lab" },
                    { 10422, 1046, "6L104", true, true, "Project and Design Lab" },
                    { 10423, 1047, "6C101", true, false, "Programming Fundamentals" },
                    { 10424, 1047, "6C102", true, false, "Discrete Mathematics" },
                    { 10425, 1047, "6C103", true, false, "Data Structures" },
                    { 10426, 1047, "6C104", true, false, "Digital Logic" },
                    { 10427, 1047, "6C105", true, false, "Communication Skills" },
                    { 10428, 1047, "6L101", true, true, "Programming Lab" },
                    { 10429, 1047, "6L102", true, true, "Digital Systems Lab" },
                    { 10430, 1047, "6L103", true, true, "Engineering Drawing Lab" },
                    { 10431, 1047, "6L104", true, true, "Project and Design Lab" },
                    { 10432, 1048, "6C101", true, false, "Programming Fundamentals" },
                    { 10433, 1048, "6C102", true, false, "Discrete Mathematics" },
                    { 10434, 1048, "6C103", true, false, "Data Structures" },
                    { 10435, 1048, "6C104", true, false, "Digital Logic" },
                    { 10436, 1048, "6C105", true, false, "Communication Skills" },
                    { 10437, 1048, "6L101", true, true, "Programming Lab" },
                    { 10438, 1048, "6L102", true, true, "Digital Systems Lab" },
                    { 10439, 1048, "6L103", true, true, "Engineering Drawing Lab" },
                    { 10440, 1048, "6L104", true, true, "Project and Design Lab" },
                    { 10441, 1049, "6C101", true, false, "Programming Fundamentals" },
                    { 10442, 1049, "6C102", true, false, "Discrete Mathematics" },
                    { 10443, 1049, "6C103", true, false, "Data Structures" },
                    { 10444, 1049, "6C104", true, false, "Digital Logic" },
                    { 10445, 1049, "6C105", true, false, "Communication Skills" },
                    { 10446, 1049, "6L101", true, true, "Programming Lab" },
                    { 10447, 1049, "6L102", true, true, "Digital Systems Lab" },
                    { 10448, 1049, "6L103", true, true, "Engineering Drawing Lab" },
                    { 10449, 1049, "6L104", true, true, "Project and Design Lab" },
                    { 10450, 1050, "6C101", true, false, "Programming Fundamentals" },
                    { 10451, 1050, "6C102", true, false, "Discrete Mathematics" },
                    { 10452, 1050, "6C103", true, false, "Data Structures" },
                    { 10453, 1050, "6C104", true, false, "Digital Logic" },
                    { 10454, 1050, "6C105", true, false, "Communication Skills" },
                    { 10455, 1050, "6L101", true, true, "Programming Lab" },
                    { 10456, 1050, "6L102", true, true, "Digital Systems Lab" },
                    { 10457, 1050, "6L103", true, true, "Engineering Drawing Lab" },
                    { 10458, 1050, "6L104", true, true, "Project and Design Lab" },
                    { 10459, 1051, "6C101", true, false, "Programming Fundamentals" },
                    { 10460, 1051, "6C102", true, false, "Discrete Mathematics" },
                    { 10461, 1051, "6C103", true, false, "Data Structures" },
                    { 10462, 1051, "6C104", true, false, "Digital Logic" },
                    { 10463, 1051, "6C105", true, false, "Communication Skills" },
                    { 10464, 1051, "6L101", true, true, "Programming Lab" },
                    { 10465, 1051, "6L102", true, true, "Digital Systems Lab" },
                    { 10466, 1051, "6L103", true, true, "Engineering Drawing Lab" },
                    { 10467, 1051, "6L104", true, true, "Project and Design Lab" },
                    { 10468, 1052, "6C101", true, false, "Programming Fundamentals" },
                    { 10469, 1052, "6C102", true, false, "Discrete Mathematics" },
                    { 10470, 1052, "6C103", true, false, "Data Structures" },
                    { 10471, 1052, "6C104", true, false, "Digital Logic" },
                    { 10472, 1052, "6C105", true, false, "Communication Skills" },
                    { 10473, 1052, "6L101", true, true, "Programming Lab" },
                    { 10474, 1052, "6L102", true, true, "Digital Systems Lab" },
                    { 10475, 1052, "6L103", true, true, "Engineering Drawing Lab" },
                    { 10476, 1052, "6L104", true, true, "Project and Design Lab" },
                    { 10477, 1053, "6C101", true, false, "Programming Fundamentals" },
                    { 10478, 1053, "6C102", true, false, "Discrete Mathematics" },
                    { 10479, 1053, "6C103", true, false, "Data Structures" },
                    { 10480, 1053, "6C104", true, false, "Digital Logic" },
                    { 10481, 1053, "6C105", true, false, "Communication Skills" },
                    { 10482, 1053, "6L101", true, true, "Programming Lab" },
                    { 10483, 1053, "6L102", true, true, "Digital Systems Lab" },
                    { 10484, 1053, "6L103", true, true, "Engineering Drawing Lab" },
                    { 10485, 1053, "6L104", true, true, "Project and Design Lab" },
                    { 10486, 1054, "7C101", true, false, "Programming Fundamentals" },
                    { 10487, 1054, "7C102", true, false, "Discrete Mathematics" },
                    { 10488, 1054, "7C103", true, false, "Data Structures" },
                    { 10489, 1054, "7C104", true, false, "Digital Logic" },
                    { 10490, 1054, "7C105", true, false, "Communication Skills" },
                    { 10491, 1054, "7L101", true, true, "Programming Lab" },
                    { 10492, 1054, "7L102", true, true, "Digital Systems Lab" },
                    { 10493, 1054, "7L103", true, true, "Engineering Drawing Lab" },
                    { 10494, 1054, "7L104", true, true, "Project and Design Lab" },
                    { 10495, 1055, "7C101", true, false, "Programming Fundamentals" },
                    { 10496, 1055, "7C102", true, false, "Discrete Mathematics" },
                    { 10497, 1055, "7C103", true, false, "Data Structures" },
                    { 10498, 1055, "7C104", true, false, "Digital Logic" },
                    { 10499, 1055, "7C105", true, false, "Communication Skills" },
                    { 10500, 1055, "7L101", true, true, "Programming Lab" },
                    { 10501, 1055, "7L102", true, true, "Digital Systems Lab" },
                    { 10502, 1055, "7L103", true, true, "Engineering Drawing Lab" },
                    { 10503, 1055, "7L104", true, true, "Project and Design Lab" },
                    { 10504, 1056, "7C101", true, false, "Programming Fundamentals" },
                    { 10505, 1056, "7C102", true, false, "Discrete Mathematics" },
                    { 10506, 1056, "7C103", true, false, "Data Structures" },
                    { 10507, 1056, "7C104", true, false, "Digital Logic" },
                    { 10508, 1056, "7C105", true, false, "Communication Skills" },
                    { 10509, 1056, "7L101", true, true, "Programming Lab" },
                    { 10510, 1056, "7L102", true, true, "Digital Systems Lab" },
                    { 10511, 1056, "7L103", true, true, "Engineering Drawing Lab" },
                    { 10512, 1056, "7L104", true, true, "Project and Design Lab" },
                    { 10513, 1057, "7C101", true, false, "Programming Fundamentals" },
                    { 10514, 1057, "7C102", true, false, "Discrete Mathematics" },
                    { 10515, 1057, "7C103", true, false, "Data Structures" },
                    { 10516, 1057, "7C104", true, false, "Digital Logic" },
                    { 10517, 1057, "7C105", true, false, "Communication Skills" },
                    { 10518, 1057, "7L101", true, true, "Programming Lab" },
                    { 10519, 1057, "7L102", true, true, "Digital Systems Lab" },
                    { 10520, 1057, "7L103", true, true, "Engineering Drawing Lab" },
                    { 10521, 1057, "7L104", true, true, "Project and Design Lab" },
                    { 10522, 1058, "7C101", true, false, "Programming Fundamentals" },
                    { 10523, 1058, "7C102", true, false, "Discrete Mathematics" },
                    { 10524, 1058, "7C103", true, false, "Data Structures" },
                    { 10525, 1058, "7C104", true, false, "Digital Logic" },
                    { 10526, 1058, "7C105", true, false, "Communication Skills" },
                    { 10527, 1058, "7L101", true, true, "Programming Lab" },
                    { 10528, 1058, "7L102", true, true, "Digital Systems Lab" },
                    { 10529, 1058, "7L103", true, true, "Engineering Drawing Lab" },
                    { 10530, 1058, "7L104", true, true, "Project and Design Lab" },
                    { 10531, 1059, "7C101", true, false, "Programming Fundamentals" },
                    { 10532, 1059, "7C102", true, false, "Discrete Mathematics" },
                    { 10533, 1059, "7C103", true, false, "Data Structures" },
                    { 10534, 1059, "7C104", true, false, "Digital Logic" },
                    { 10535, 1059, "7C105", true, false, "Communication Skills" },
                    { 10536, 1059, "7L101", true, true, "Programming Lab" },
                    { 10537, 1059, "7L102", true, true, "Digital Systems Lab" },
                    { 10538, 1059, "7L103", true, true, "Engineering Drawing Lab" },
                    { 10539, 1059, "7L104", true, true, "Project and Design Lab" },
                    { 10540, 1060, "7C101", true, false, "Programming Fundamentals" },
                    { 10541, 1060, "7C102", true, false, "Discrete Mathematics" },
                    { 10542, 1060, "7C103", true, false, "Data Structures" },
                    { 10543, 1060, "7C104", true, false, "Digital Logic" },
                    { 10544, 1060, "7C105", true, false, "Communication Skills" },
                    { 10545, 1060, "7L101", true, true, "Programming Lab" },
                    { 10546, 1060, "7L102", true, true, "Digital Systems Lab" },
                    { 10547, 1060, "7L103", true, true, "Engineering Drawing Lab" },
                    { 10548, 1060, "7L104", true, true, "Project and Design Lab" },
                    { 10549, 1061, "7C101", true, false, "Programming Fundamentals" },
                    { 10550, 1061, "7C102", true, false, "Discrete Mathematics" },
                    { 10551, 1061, "7C103", true, false, "Data Structures" },
                    { 10552, 1061, "7C104", true, false, "Digital Logic" },
                    { 10553, 1061, "7C105", true, false, "Communication Skills" },
                    { 10554, 1061, "7L101", true, true, "Programming Lab" },
                    { 10555, 1061, "7L102", true, true, "Digital Systems Lab" },
                    { 10556, 1061, "7L103", true, true, "Engineering Drawing Lab" },
                    { 10557, 1061, "7L104", true, true, "Project and Design Lab" },
                    { 10558, 1062, "7C101", true, false, "Programming Fundamentals" },
                    { 10559, 1062, "7C102", true, false, "Discrete Mathematics" },
                    { 10560, 1062, "7C103", true, false, "Data Structures" },
                    { 10561, 1062, "7C104", true, false, "Digital Logic" },
                    { 10562, 1062, "7C105", true, false, "Communication Skills" },
                    { 10563, 1062, "7L101", true, true, "Programming Lab" },
                    { 10564, 1062, "7L102", true, true, "Digital Systems Lab" },
                    { 10565, 1062, "7L103", true, true, "Engineering Drawing Lab" },
                    { 10566, 1062, "7L104", true, true, "Project and Design Lab" },
                    { 10567, 1063, "8C101", true, false, "Programming Fundamentals" },
                    { 10568, 1063, "8C102", true, false, "Discrete Mathematics" },
                    { 10569, 1063, "8C103", true, false, "Data Structures" },
                    { 10570, 1063, "8C104", true, false, "Digital Logic" },
                    { 10571, 1063, "8C105", true, false, "Communication Skills" },
                    { 10572, 1063, "8L101", true, true, "Programming Lab" },
                    { 10573, 1063, "8L102", true, true, "Digital Systems Lab" },
                    { 10574, 1063, "8L103", true, true, "Engineering Drawing Lab" },
                    { 10575, 1063, "8L104", true, true, "Project and Design Lab" },
                    { 10576, 1064, "8C101", true, false, "Programming Fundamentals" },
                    { 10577, 1064, "8C102", true, false, "Discrete Mathematics" },
                    { 10578, 1064, "8C103", true, false, "Data Structures" },
                    { 10579, 1064, "8C104", true, false, "Digital Logic" },
                    { 10580, 1064, "8C105", true, false, "Communication Skills" },
                    { 10581, 1064, "8L101", true, true, "Programming Lab" },
                    { 10582, 1064, "8L102", true, true, "Digital Systems Lab" },
                    { 10583, 1064, "8L103", true, true, "Engineering Drawing Lab" },
                    { 10584, 1064, "8L104", true, true, "Project and Design Lab" },
                    { 10585, 1065, "8C101", true, false, "Programming Fundamentals" },
                    { 10586, 1065, "8C102", true, false, "Discrete Mathematics" },
                    { 10587, 1065, "8C103", true, false, "Data Structures" },
                    { 10588, 1065, "8C104", true, false, "Digital Logic" },
                    { 10589, 1065, "8C105", true, false, "Communication Skills" },
                    { 10590, 1065, "8L101", true, true, "Programming Lab" },
                    { 10591, 1065, "8L102", true, true, "Digital Systems Lab" },
                    { 10592, 1065, "8L103", true, true, "Engineering Drawing Lab" },
                    { 10593, 1065, "8L104", true, true, "Project and Design Lab" },
                    { 10594, 1066, "8C101", true, false, "Programming Fundamentals" },
                    { 10595, 1066, "8C102", true, false, "Discrete Mathematics" },
                    { 10596, 1066, "8C103", true, false, "Data Structures" },
                    { 10597, 1066, "8C104", true, false, "Digital Logic" },
                    { 10598, 1066, "8C105", true, false, "Communication Skills" },
                    { 10599, 1066, "8L101", true, true, "Programming Lab" },
                    { 10600, 1066, "8L102", true, true, "Digital Systems Lab" },
                    { 10601, 1066, "8L103", true, true, "Engineering Drawing Lab" },
                    { 10602, 1066, "8L104", true, true, "Project and Design Lab" },
                    { 10603, 1067, "8C101", true, false, "Programming Fundamentals" },
                    { 10604, 1067, "8C102", true, false, "Discrete Mathematics" },
                    { 10605, 1067, "8C103", true, false, "Data Structures" },
                    { 10606, 1067, "8C104", true, false, "Digital Logic" },
                    { 10607, 1067, "8C105", true, false, "Communication Skills" },
                    { 10608, 1067, "8L101", true, true, "Programming Lab" },
                    { 10609, 1067, "8L102", true, true, "Digital Systems Lab" },
                    { 10610, 1067, "8L103", true, true, "Engineering Drawing Lab" },
                    { 10611, 1067, "8L104", true, true, "Project and Design Lab" },
                    { 10612, 1068, "8C101", true, false, "Programming Fundamentals" },
                    { 10613, 1068, "8C102", true, false, "Discrete Mathematics" },
                    { 10614, 1068, "8C103", true, false, "Data Structures" },
                    { 10615, 1068, "8C104", true, false, "Digital Logic" },
                    { 10616, 1068, "8C105", true, false, "Communication Skills" },
                    { 10617, 1068, "8L101", true, true, "Programming Lab" },
                    { 10618, 1068, "8L102", true, true, "Digital Systems Lab" },
                    { 10619, 1068, "8L103", true, true, "Engineering Drawing Lab" },
                    { 10620, 1068, "8L104", true, true, "Project and Design Lab" },
                    { 10621, 1069, "8C101", true, false, "Programming Fundamentals" },
                    { 10622, 1069, "8C102", true, false, "Discrete Mathematics" },
                    { 10623, 1069, "8C103", true, false, "Data Structures" },
                    { 10624, 1069, "8C104", true, false, "Digital Logic" },
                    { 10625, 1069, "8C105", true, false, "Communication Skills" },
                    { 10626, 1069, "8L101", true, true, "Programming Lab" },
                    { 10627, 1069, "8L102", true, true, "Digital Systems Lab" },
                    { 10628, 1069, "8L103", true, true, "Engineering Drawing Lab" },
                    { 10629, 1069, "8L104", true, true, "Project and Design Lab" },
                    { 10630, 1070, "8C101", true, false, "Programming Fundamentals" },
                    { 10631, 1070, "8C102", true, false, "Discrete Mathematics" },
                    { 10632, 1070, "8C103", true, false, "Data Structures" },
                    { 10633, 1070, "8C104", true, false, "Digital Logic" },
                    { 10634, 1070, "8C105", true, false, "Communication Skills" },
                    { 10635, 1070, "8L101", true, true, "Programming Lab" },
                    { 10636, 1070, "8L102", true, true, "Digital Systems Lab" },
                    { 10637, 1070, "8L103", true, true, "Engineering Drawing Lab" },
                    { 10638, 1070, "8L104", true, true, "Project and Design Lab" },
                    { 10639, 1071, "8C101", true, false, "Programming Fundamentals" },
                    { 10640, 1071, "8C102", true, false, "Discrete Mathematics" },
                    { 10641, 1071, "8C103", true, false, "Data Structures" },
                    { 10642, 1071, "8C104", true, false, "Digital Logic" },
                    { 10643, 1071, "8C105", true, false, "Communication Skills" },
                    { 10644, 1071, "8L101", true, true, "Programming Lab" },
                    { 10645, 1071, "8L102", true, true, "Digital Systems Lab" },
                    { 10646, 1071, "8L103", true, true, "Engineering Drawing Lab" },
                    { 10647, 1071, "8L104", true, true, "Project and Design Lab" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_PastPapers_AcademicCourseId",
                table: "PastPapers",
                column: "AcademicCourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_AcademicCourseId",
                table: "Notes",
                column: "AcademicCourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Lectures_AcademicCourseId",
                table: "Lectures",
                column: "AcademicCourseId");

            migrationBuilder.CreateIndex(
                name: "IX_AcademicCourses_AcademicSemesterId_CourseCode",
                table: "AcademicCourses",
                columns: new[] { "AcademicSemesterId", "CourseCode" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Lectures_AcademicCourses_AcademicCourseId",
                table: "Lectures",
                column: "AcademicCourseId",
                principalTable: "AcademicCourses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_AcademicCourses_AcademicCourseId",
                table: "Notes",
                column: "AcademicCourseId",
                principalTable: "AcademicCourses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PastPapers_AcademicCourses_AcademicCourseId",
                table: "PastPapers",
                column: "AcademicCourseId",
                principalTable: "AcademicCourses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lectures_AcademicCourses_AcademicCourseId",
                table: "Lectures");

            migrationBuilder.DropForeignKey(
                name: "FK_Notes_AcademicCourses_AcademicCourseId",
                table: "Notes");

            migrationBuilder.DropForeignKey(
                name: "FK_PastPapers_AcademicCourses_AcademicCourseId",
                table: "PastPapers");

            migrationBuilder.DropTable(
                name: "AcademicCourses");

            migrationBuilder.DropIndex(
                name: "IX_PastPapers_AcademicCourseId",
                table: "PastPapers");

            migrationBuilder.DropIndex(
                name: "IX_Notes_AcademicCourseId",
                table: "Notes");

            migrationBuilder.DropIndex(
                name: "IX_Lectures_AcademicCourseId",
                table: "Lectures");

            migrationBuilder.DeleteData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 1000);

            migrationBuilder.DeleteData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 1001);

            migrationBuilder.DeleteData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 1002);

            migrationBuilder.DeleteData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 1003);

            migrationBuilder.DeleteData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 1004);

            migrationBuilder.DeleteData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 1005);

            migrationBuilder.DeleteData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 1006);

            migrationBuilder.DeleteData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 1007);

            migrationBuilder.DeleteData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 1008);

            migrationBuilder.DeleteData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 1009);

            migrationBuilder.DeleteData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 1010);

            migrationBuilder.DeleteData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 1011);

            migrationBuilder.DeleteData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 1012);

            migrationBuilder.DeleteData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 1013);

            migrationBuilder.DeleteData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 1014);

            migrationBuilder.DeleteData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 1015);

            migrationBuilder.DeleteData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 1016);

            migrationBuilder.DeleteData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 1017);

            migrationBuilder.DeleteData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 1018);

            migrationBuilder.DeleteData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 1019);

            migrationBuilder.DeleteData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 1020);

            migrationBuilder.DeleteData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 1021);

            migrationBuilder.DeleteData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 1022);

            migrationBuilder.DeleteData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 1023);

            migrationBuilder.DeleteData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 1024);

            migrationBuilder.DeleteData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 1025);

            migrationBuilder.DeleteData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 1026);

            migrationBuilder.DeleteData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 1027);

            migrationBuilder.DeleteData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 1028);

            migrationBuilder.DeleteData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 1029);

            migrationBuilder.DeleteData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 1030);

            migrationBuilder.DeleteData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 1031);

            migrationBuilder.DeleteData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 1032);

            migrationBuilder.DeleteData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 1033);

            migrationBuilder.DeleteData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 1034);

            migrationBuilder.DeleteData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 1035);

            migrationBuilder.DeleteData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 1036);

            migrationBuilder.DeleteData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 1037);

            migrationBuilder.DeleteData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 1038);

            migrationBuilder.DeleteData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 1039);

            migrationBuilder.DeleteData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 1040);

            migrationBuilder.DeleteData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 1041);

            migrationBuilder.DeleteData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 1042);

            migrationBuilder.DeleteData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 1043);

            migrationBuilder.DeleteData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 1044);

            migrationBuilder.DeleteData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 1045);

            migrationBuilder.DeleteData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 1046);

            migrationBuilder.DeleteData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 1047);

            migrationBuilder.DeleteData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 1048);

            migrationBuilder.DeleteData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 1049);

            migrationBuilder.DeleteData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 1050);

            migrationBuilder.DeleteData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 1051);

            migrationBuilder.DeleteData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 1052);

            migrationBuilder.DeleteData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 1053);

            migrationBuilder.DeleteData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 1054);

            migrationBuilder.DeleteData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 1055);

            migrationBuilder.DeleteData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 1056);

            migrationBuilder.DeleteData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 1057);

            migrationBuilder.DeleteData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 1058);

            migrationBuilder.DeleteData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 1059);

            migrationBuilder.DeleteData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 1060);

            migrationBuilder.DeleteData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 1061);

            migrationBuilder.DeleteData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 1062);

            migrationBuilder.DeleteData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 1063);

            migrationBuilder.DeleteData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 1064);

            migrationBuilder.DeleteData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 1065);

            migrationBuilder.DeleteData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 1066);

            migrationBuilder.DeleteData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 1067);

            migrationBuilder.DeleteData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 1068);

            migrationBuilder.DeleteData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 1069);

            migrationBuilder.DeleteData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 1070);

            migrationBuilder.DeleteData(
                table: "AcademicSemesters",
                keyColumn: "Id",
                keyValue: 1071);

            migrationBuilder.DropColumn(
                name: "AcademicCourseId",
                table: "PastPapers");

            migrationBuilder.DropColumn(
                name: "AcademicCourseId",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "AcademicCourseId",
                table: "Lectures");

            migrationBuilder.DropColumn(
                name: "AcademicYear",
                table: "AcademicSemesters");

            migrationBuilder.DropColumn(
                name: "TermName",
                table: "AcademicSemesters");
        }
    }
}
