using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace student_resource_hub.Migrations
{
    /// <inheritdoc />
    public partial class AddUniversityAcademicCatalog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AcademicDepartmentId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AcademicSemesterId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UniversityId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AcademicDepartmentId",
                table: "PastPapers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AcademicSemesterId",
                table: "PastPapers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AcademicDepartmentId",
                table: "Notes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AcademicSemesterId",
                table: "Notes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AcademicDepartmentId",
                table: "Lectures",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AcademicSemesterId",
                table: "Lectures",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Universities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    LogoUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Universities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AcademicDepartments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UniversityId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
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
                });

            migrationBuilder.CreateTable(
                name: "AcademicSemesters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
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
                });

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
                name: "IX_Users_AcademicDepartmentId",
                table: "Users",
                column: "AcademicDepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_AcademicSemesterId",
                table: "Users",
                column: "AcademicSemesterId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UniversityId",
                table: "Users",
                column: "UniversityId");

            migrationBuilder.CreateIndex(
                name: "IX_PastPapers_AcademicDepartmentId",
                table: "PastPapers",
                column: "AcademicDepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_PastPapers_AcademicSemesterId",
                table: "PastPapers",
                column: "AcademicSemesterId");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_AcademicDepartmentId",
                table: "Notes",
                column: "AcademicDepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_AcademicSemesterId",
                table: "Notes",
                column: "AcademicSemesterId");

            migrationBuilder.CreateIndex(
                name: "IX_Lectures_AcademicDepartmentId",
                table: "Lectures",
                column: "AcademicDepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Lectures_AcademicSemesterId",
                table: "Lectures",
                column: "AcademicSemesterId");

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
                name: "IX_Universities_Name",
                table: "Universities",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Lectures_AcademicDepartments_AcademicDepartmentId",
                table: "Lectures",
                column: "AcademicDepartmentId",
                principalTable: "AcademicDepartments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Lectures_AcademicSemesters_AcademicSemesterId",
                table: "Lectures",
                column: "AcademicSemesterId",
                principalTable: "AcademicSemesters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_AcademicDepartments_AcademicDepartmentId",
                table: "Notes",
                column: "AcademicDepartmentId",
                principalTable: "AcademicDepartments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_AcademicSemesters_AcademicSemesterId",
                table: "Notes",
                column: "AcademicSemesterId",
                principalTable: "AcademicSemesters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PastPapers_AcademicDepartments_AcademicDepartmentId",
                table: "PastPapers",
                column: "AcademicDepartmentId",
                principalTable: "AcademicDepartments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PastPapers_AcademicSemesters_AcademicSemesterId",
                table: "PastPapers",
                column: "AcademicSemesterId",
                principalTable: "AcademicSemesters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_AcademicDepartments_AcademicDepartmentId",
                table: "Users",
                column: "AcademicDepartmentId",
                principalTable: "AcademicDepartments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_AcademicSemesters_AcademicSemesterId",
                table: "Users",
                column: "AcademicSemesterId",
                principalTable: "AcademicSemesters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Universities_UniversityId",
                table: "Users",
                column: "UniversityId",
                principalTable: "Universities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lectures_AcademicDepartments_AcademicDepartmentId",
                table: "Lectures");

            migrationBuilder.DropForeignKey(
                name: "FK_Lectures_AcademicSemesters_AcademicSemesterId",
                table: "Lectures");

            migrationBuilder.DropForeignKey(
                name: "FK_Notes_AcademicDepartments_AcademicDepartmentId",
                table: "Notes");

            migrationBuilder.DropForeignKey(
                name: "FK_Notes_AcademicSemesters_AcademicSemesterId",
                table: "Notes");

            migrationBuilder.DropForeignKey(
                name: "FK_PastPapers_AcademicDepartments_AcademicDepartmentId",
                table: "PastPapers");

            migrationBuilder.DropForeignKey(
                name: "FK_PastPapers_AcademicSemesters_AcademicSemesterId",
                table: "PastPapers");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_AcademicDepartments_AcademicDepartmentId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_AcademicSemesters_AcademicSemesterId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Universities_UniversityId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "AcademicSemesters");

            migrationBuilder.DropTable(
                name: "AcademicDepartments");

            migrationBuilder.DropTable(
                name: "Universities");

            migrationBuilder.DropIndex(
                name: "IX_Users_AcademicDepartmentId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_AcademicSemesterId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_UniversityId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_PastPapers_AcademicDepartmentId",
                table: "PastPapers");

            migrationBuilder.DropIndex(
                name: "IX_PastPapers_AcademicSemesterId",
                table: "PastPapers");

            migrationBuilder.DropIndex(
                name: "IX_Notes_AcademicDepartmentId",
                table: "Notes");

            migrationBuilder.DropIndex(
                name: "IX_Notes_AcademicSemesterId",
                table: "Notes");

            migrationBuilder.DropIndex(
                name: "IX_Lectures_AcademicDepartmentId",
                table: "Lectures");

            migrationBuilder.DropIndex(
                name: "IX_Lectures_AcademicSemesterId",
                table: "Lectures");

            migrationBuilder.DropColumn(
                name: "AcademicDepartmentId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AcademicSemesterId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UniversityId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AcademicDepartmentId",
                table: "PastPapers");

            migrationBuilder.DropColumn(
                name: "AcademicSemesterId",
                table: "PastPapers");

            migrationBuilder.DropColumn(
                name: "AcademicDepartmentId",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "AcademicSemesterId",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "AcademicDepartmentId",
                table: "Lectures");

            migrationBuilder.DropColumn(
                name: "AcademicSemesterId",
                table: "Lectures");
        }
    }
}
