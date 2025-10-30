using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdleDB.Migrations
{
    /// <inheritdoc />
    public partial class AddEarlyLevelEnemies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Enemies",
                columns: ["Id", "Name", "PhysicalDamage", "AetherDamage", "FireDamage", "ColdDamage", "PoisonDamage", "CritRating", "CritMultiplier", "ActionSpeed", "Health", "Armour", "Barrier", "Evasion", "FireResistance", "ColdResistance", "PoisonResistance"],
                values: new object[,]
                {
                    { 1, "Mud Crab", 1m, 0m, 0m, 0m, 0m, 0m, 2m, 1m, 750, 0, 0, 0m, 0m, 0m, 0m },
                    { 2, "Snapping Turtle", 2m, 0m, 0m, 0m, 0m, 0m, 2m, 1.5m, 1000, 1000, 0, 0m, 0m, 0m, 0m },
                    { 3, "Jellyfish", 0m, 1m, 0m, 0m, 2m, 0m, 2m, 1m, 900, 0, 1000, 0m, 0m, 0m, 0m },
                    { 4, "Giant Crab", 7m, 0m, 0m, 0m, 0m, 0m, 2m, 1.5m, 4000, 1000, 1000, 0m, 0m, 0m, 0m },
                });

            migrationBuilder.UpdateData(
                table: "Levels",
                keyColumn: "Id",
                keyValue: 1,
                column: "FrontEnemyId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Levels",
                keyColumn: "Id",
                keyValue: 2,
                columns: ["FrontEnemyId", "MiddleEnemyId"],
                values: [1, 1]);

            migrationBuilder.UpdateData(
                table: "Levels",
                keyColumn: "Id",
                keyValue: 3,
                columns: ["FrontEnemyId", "MiddleEnemyId"],
                values: [2, 1]);

            migrationBuilder.UpdateData(
                table: "Levels",
                keyColumn: "Id",
                keyValue: 4,
                columns: ["FrontEnemyId", "MiddleEnemyId"],
                values: [1, 3]);

            migrationBuilder.UpdateData(
                table: "Levels",
                keyColumn: "Id",
                keyValue: 5,
                columns: ["FrontEnemyId", "MiddleEnemyId", "BackEnemyId"],
                values: [2, 1, 1]);

            migrationBuilder.UpdateData(
                table: "Levels",
                keyColumn: "Id",
                keyValue: 6,
                columns: ["FrontEnemyId", "MiddleEnemyId", "BackEnemyId"],
                values: [1, 1, 3]);

            migrationBuilder.UpdateData(
                table: "Levels",
                keyColumn: "Id",
                keyValue: 7,
                columns: ["FrontEnemyId", "MiddleEnemyId", "BackEnemyId"],
                values: [2, 1, 3]);

            migrationBuilder.UpdateData(
                table: "Levels",
                keyColumn: "Id",
                keyValue: 8,
                columns: ["FrontEnemyId", "MiddleEnemyId", "BackEnemyId"],
                values: [2, 2, 3]);

            migrationBuilder.UpdateData(
                table: "Levels",
                keyColumn: "Id",
                keyValue: 9,
                columns: ["FrontEnemyId", "MiddleEnemyId", "BackEnemyId"],
                values: [2, 3, 3]);

            migrationBuilder.UpdateData(
                table: "Levels",
                keyColumn: "Id",
                keyValue: 10,
                column: "FrontEnemyId",
                value: 4);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Levels",
                keyColumn: "Id",
                keyValue: 1,
                column: "FrontEnemyId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Levels",
                keyColumn: "Id",
                keyValue: 2,
                columns: ["FrontEnemyId", "MiddleEnemyId"],
                values: [null, null]);

            migrationBuilder.UpdateData(
                table: "Levels",
                keyColumn: "Id",
                keyValue: 3,
                columns: ["FrontEnemyId", "MiddleEnemyId"],
                values: [null, null]);

            migrationBuilder.UpdateData(
                table: "Levels",
                keyColumn: "Id",
                keyValue: 4,
                columns: ["FrontEnemyId", "MiddleEnemyId"],
                values: [null, null]);

            migrationBuilder.UpdateData(
                table: "Levels",
                keyColumn: "Id",
                keyValue: 5,
                columns: ["FrontEnemyId", "MiddleEnemyId", "BackEnemyId"],
                values: [null, null, null]);

            migrationBuilder.UpdateData(
                table: "Levels",
                keyColumn: "Id",
                keyValue: 6,
                columns: ["FrontEnemyId", "MiddleEnemyId", "BackEnemyId"],
                values: [null, null, null]);

            migrationBuilder.UpdateData(
                table: "Levels",
                keyColumn: "Id",
                keyValue: 7,
                columns: ["FrontEnemyId", "MiddleEnemyId", "BackEnemyId"],
                values: [null, null, null]);

            migrationBuilder.UpdateData(
                table: "Levels",
                keyColumn: "Id",
                keyValue: 8,
                columns: ["FrontEnemyId", "MiddleEnemyId", "BackEnemyId"],
                values: [null, null, null]);

            migrationBuilder.UpdateData(
                table: "Levels",
                keyColumn: "Id",
                keyValue: 9,
                columns: ["FrontEnemyId", "MiddleEnemyId", "BackEnemyId"],
                values: [null, null, null]);

            migrationBuilder.UpdateData(
                table: "Levels",
                keyColumn: "Id",
                keyValue: 10,
                column: "FrontEnemyId",
                value: null);

            migrationBuilder.DeleteData(
                table: "Enemies",
                keyColumn: "Id",
                keyValues: [1, 2, 3, 4]);
        }
    }
}
