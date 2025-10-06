using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdleDB.Migrations
{
    /// <inheritdoc />
    public partial class ReplaceCharacterLevelWithExperience : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Level",
                table: "Characters",
                newName: "Experience");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Experience",
                table: "Characters",
                newName: "Level");
        }
    }
}
