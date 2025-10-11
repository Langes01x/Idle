using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdleDB.Migrations
{
    /// <inheritdoc />
    public partial class AddParties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Parties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AccountId = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    BackCharacterId = table.Column<int>(type: "INTEGER", nullable: true),
                    MiddleCharacterId = table.Column<int>(type: "INTEGER", nullable: true),
                    FrontCharacterId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Parties_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Parties_Characters_BackCharacterId",
                        column: x => x.BackCharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Parties_Characters_FrontCharacterId",
                        column: x => x.FrontCharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Parties_Characters_MiddleCharacterId",
                        column: x => x.MiddleCharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Parties_AccountId",
                table: "Parties",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Parties_BackCharacterId",
                table: "Parties",
                column: "BackCharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_Parties_FrontCharacterId",
                table: "Parties",
                column: "FrontCharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_Parties_MiddleCharacterId",
                table: "Parties",
                column: "MiddleCharacterId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Parties");
        }
    }
}
