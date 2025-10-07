using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdleDB.Migrations
{
    /// <inheritdoc />
    public partial class AddCharacterSortOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CharacterSortOrder",
                table: "Accounts",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CharacterSortOrder",
                table: "Accounts");
        }
    }
}
