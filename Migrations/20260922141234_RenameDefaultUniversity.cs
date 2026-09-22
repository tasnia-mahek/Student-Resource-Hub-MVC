using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace student_resource_hub.Migrations
{
    /// <inheritdoc />
    public partial class RenameDefaultUniversity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE Universities SET Name = 'Ahsanullah University of Science and Technology' WHERE Id = 1 AND Name = 'Default University';");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE Universities SET Name = 'Default University' WHERE Id = 1 AND Name = 'Ahsanullah University of Science and Technology';");
        }
    }
}
