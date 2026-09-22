using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace student_resource_hub.Migrations
{
    /// <inheritdoc />
    public partial class AddNestedStudyFoldersV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParentFolderId",
                table: "StudyFolders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudyFolders_ParentFolderId",
                table: "StudyFolders",
                column: "ParentFolderId");

            migrationBuilder.CreateIndex(
                name: "IX_StudyFolders_StudySessionId_ParentFolderId_Name",
                table: "StudyFolders",
                columns: new[] { "StudySessionId", "ParentFolderId", "Name" },
                unique: true);

            migrationBuilder.DropIndex(
                name: "IX_StudyFolders_StudySessionId_Name",
                table: "StudyFolders");

            migrationBuilder.AddForeignKey(
                name: "FK_StudyFolders_StudyFolders_ParentFolderId",
                table: "StudyFolders",
                column: "ParentFolderId",
                principalTable: "StudyFolders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudyFolders_StudyFolders_ParentFolderId",
                table: "StudyFolders");

            migrationBuilder.DropIndex(
                name: "IX_StudyFolders_ParentFolderId",
                table: "StudyFolders");

            migrationBuilder.DropIndex(
                name: "IX_StudyFolders_StudySessionId_ParentFolderId_Name",
                table: "StudyFolders");

            migrationBuilder.DropColumn(
                name: "ParentFolderId",
                table: "StudyFolders");

            migrationBuilder.CreateIndex(
                name: "IX_StudyFolders_StudySessionId_Name",
                table: "StudyFolders",
                columns: new[] { "StudySessionId", "Name" },
                unique: true);
        }
    }
}
