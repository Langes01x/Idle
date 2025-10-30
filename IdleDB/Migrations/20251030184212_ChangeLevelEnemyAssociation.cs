using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdleDB.Migrations
{
    /// <inheritdoc />
    public partial class ChangeLevelEnemyAssociation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enemies_Levels_LevelId",
                table: "Enemies");

            migrationBuilder.DropIndex(
                name: "IX_Enemies_LevelId",
                table: "Enemies");

            migrationBuilder.DropColumn(
                name: "LevelId",
                table: "Enemies");

            migrationBuilder.AddColumn<int>(
                name: "BackEnemyId",
                table: "Levels",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FrontEnemyId",
                table: "Levels",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MiddleEnemyId",
                table: "Levels",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Levels_BackEnemyId",
                table: "Levels",
                column: "BackEnemyId");

            migrationBuilder.CreateIndex(
                name: "IX_Levels_FrontEnemyId",
                table: "Levels",
                column: "FrontEnemyId");

            migrationBuilder.CreateIndex(
                name: "IX_Levels_MiddleEnemyId",
                table: "Levels",
                column: "MiddleEnemyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Levels_Enemies_BackEnemyId",
                table: "Levels",
                column: "BackEnemyId",
                principalTable: "Enemies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Levels_Enemies_FrontEnemyId",
                table: "Levels",
                column: "FrontEnemyId",
                principalTable: "Enemies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Levels_Enemies_MiddleEnemyId",
                table: "Levels",
                column: "MiddleEnemyId",
                principalTable: "Enemies",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Levels_Enemies_BackEnemyId",
                table: "Levels");

            migrationBuilder.DropForeignKey(
                name: "FK_Levels_Enemies_FrontEnemyId",
                table: "Levels");

            migrationBuilder.DropForeignKey(
                name: "FK_Levels_Enemies_MiddleEnemyId",
                table: "Levels");

            migrationBuilder.DropIndex(
                name: "IX_Levels_BackEnemyId",
                table: "Levels");

            migrationBuilder.DropIndex(
                name: "IX_Levels_FrontEnemyId",
                table: "Levels");

            migrationBuilder.DropIndex(
                name: "IX_Levels_MiddleEnemyId",
                table: "Levels");

            migrationBuilder.DropColumn(
                name: "BackEnemyId",
                table: "Levels");

            migrationBuilder.DropColumn(
                name: "FrontEnemyId",
                table: "Levels");

            migrationBuilder.DropColumn(
                name: "MiddleEnemyId",
                table: "Levels");

            migrationBuilder.AddColumn<int>(
                name: "LevelId",
                table: "Enemies",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Enemies_LevelId",
                table: "Enemies",
                column: "LevelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Enemies_Levels_LevelId",
                table: "Enemies",
                column: "LevelId",
                principalTable: "Levels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
