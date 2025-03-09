using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InMindLab5.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AllowedForGradeAssignments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "GradePointAverage",
                table: "Students",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Grade",
                table: "Enrollments",
                type: "real",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GradePointAverage",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Grade",
                table: "Enrollments");
        }
    }
}
