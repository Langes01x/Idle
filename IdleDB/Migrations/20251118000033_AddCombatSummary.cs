using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdleDB.Migrations
{
    /// <inheritdoc />
    public partial class AddCombatSummary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CharacterSnapshots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Level = table.Column<int>(type: "INTEGER", nullable: false),
                    Health = table.Column<int>(type: "INTEGER", nullable: false),
                    Rarity = table.Column<int>(type: "INTEGER", nullable: false),
                    Class = table.Column<int>(type: "INTEGER", nullable: false),
                    FirstName = table.Column<int>(type: "INTEGER", nullable: false),
                    LastName = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterSnapshots", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CombatSummaries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AccountId = table.Column<string>(type: "TEXT", nullable: false),
                    LevelId = table.Column<int>(type: "INTEGER", nullable: false),
                    FrontCharacterId = table.Column<int>(type: "INTEGER", nullable: true),
                    MiddleCharacterId = table.Column<int>(type: "INTEGER", nullable: true),
                    BackCharacterId = table.Column<int>(type: "INTEGER", nullable: true),
                    Result = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CombatSummaries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CombatSummaries_CharacterSnapshots_BackCharacterId",
                        column: x => x.BackCharacterId,
                        principalTable: "CharacterSnapshots",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CombatSummaries_CharacterSnapshots_FrontCharacterId",
                        column: x => x.FrontCharacterId,
                        principalTable: "CharacterSnapshots",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CombatSummaries_CharacterSnapshots_MiddleCharacterId",
                        column: x => x.MiddleCharacterId,
                        principalTable: "CharacterSnapshots",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CombatSummaries_Levels_LevelId",
                        column: x => x.LevelId,
                        principalTable: "Levels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CombatActions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CombatSummaryId = table.Column<int>(type: "INTEGER", nullable: false),
                    Time = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    AttackerSide = table.Column<int>(type: "INTEGER", nullable: false),
                    AttackerPosition = table.Column<int>(type: "INTEGER", nullable: false),
                    DefenderPosition = table.Column<int>(type: "INTEGER", nullable: false),
                    IsDefenderDead = table.Column<bool>(type: "INTEGER", nullable: false),
                    PhysicalDamageDealt = table.Column<decimal>(type: "TEXT", nullable: false),
                    AetherDamageDealt = table.Column<decimal>(type: "TEXT", nullable: false),
                    FireDamageDealt = table.Column<decimal>(type: "TEXT", nullable: false),
                    ColdDamageDealt = table.Column<decimal>(type: "TEXT", nullable: false),
                    PoisonDamageDealt = table.Column<decimal>(type: "TEXT", nullable: false),
                    IsCrit = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsMiss = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CombatActions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CombatActions_CombatSummaries_CombatSummaryId",
                        column: x => x.CombatSummaryId,
                        principalTable: "CombatSummaries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CombatActions_CombatSummaryId",
                table: "CombatActions",
                column: "CombatSummaryId");

            migrationBuilder.CreateIndex(
                name: "IX_CombatSummaries_BackCharacterId",
                table: "CombatSummaries",
                column: "BackCharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_CombatSummaries_FrontCharacterId",
                table: "CombatSummaries",
                column: "FrontCharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_CombatSummaries_LevelId",
                table: "CombatSummaries",
                column: "LevelId");

            migrationBuilder.CreateIndex(
                name: "IX_CombatSummaries_MiddleCharacterId",
                table: "CombatSummaries",
                column: "MiddleCharacterId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CombatActions");

            migrationBuilder.DropTable(
                name: "CombatSummaries");

            migrationBuilder.DropTable(
                name: "CharacterSnapshots");
        }
    }
}
