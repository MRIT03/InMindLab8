using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InMindLab5.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedPfpUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProfilePictureUrl",
                table: "Students",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfilePictureUrl",
                table: "Students");
        }
    }
}
