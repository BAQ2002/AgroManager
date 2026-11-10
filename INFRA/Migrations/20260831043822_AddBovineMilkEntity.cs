using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace INFRA.Migrations
{
    /// <inheritdoc />
    public partial class AddBovineMilkEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MilkRecords");

            migrationBuilder.CreateTable(
                name: "BovineMilkRecords",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BovineId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    OccurrenceDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Liters = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BovineMilkRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BovineMilkRecords_Bovines_BovineId",
                        column: x => x.BovineId,
                        principalTable: "Bovines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BovineMilkRecords_BovineId_OccurrenceDate",
                table: "BovineMilkRecords",
                columns: new[] { "BovineId", "OccurrenceDate" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BovineMilkRecords");

            migrationBuilder.CreateTable(
                name: "MilkRecords",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BovineId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Liters = table.Column<float>(type: "real", nullable: false),
                    OccurrenceDate = table.Column<DateOnly>(type: "date", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MilkRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MilkRecords_Bovines_BovineId",
                        column: x => x.BovineId,
                        principalTable: "Bovines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MilkRecords_BovineId",
                table: "MilkRecords",
                column: "BovineId");
        }
    }
}
