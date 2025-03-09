using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InMindLab5.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addedFKConstraintBetweenAdminAndCourse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Courses_AdminId",
                table: "Courses",
                column: "AdminId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Admins_AdminId",
                table: "Courses",
                column: "AdminId",
                principalTable: "Admins",
                principalColumn: "AdminId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Admins_AdminId",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_Courses_AdminId",
                table: "Courses");
        }
    }
}
