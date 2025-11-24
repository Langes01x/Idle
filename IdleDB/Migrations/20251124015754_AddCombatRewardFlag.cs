using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdleDB.Migrations
{
    /// <inheritdoc />
    public partial class AddCombatRewardFlag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "RewardsGiven",
                table: "CombatSummaries",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_CombatSummaries_AccountId",
                table: "CombatSummaries",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_CombatSummaries_Accounts_AccountId",
                table: "CombatSummaries",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CombatSummaries_Accounts_AccountId",
                table: "CombatSummaries");

            migrationBuilder.DropIndex(
                name: "IX_CombatSummaries_AccountId",
                table: "CombatSummaries");

            migrationBuilder.DropColumn(
                name: "RewardsGiven",
                table: "CombatSummaries");
        }
    }
}
