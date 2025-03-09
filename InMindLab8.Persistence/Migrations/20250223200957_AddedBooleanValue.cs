using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InMindLab5.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedBooleanValue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "canApplyToFrance",
                table: "Students",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "canApplyToFrance",
                table: "Students");
        }
    }
}
