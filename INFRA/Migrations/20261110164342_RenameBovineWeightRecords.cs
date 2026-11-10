using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace INFRA.Migrations
{
    /// <inheritdoc />
    public partial class RenameBovineWeightRecords : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BovineWeightRecords_Bovines_BovineId",
                table: "BovineWeightRecords");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BovineWeightRecords",
                table: "BovineWeightRecords");

            migrationBuilder.RenameTable(
                name: "BovineWeightRecords",
                newName: "BovineRecordsWeight");

            migrationBuilder.RenameIndex(
                name: "IX_BovineWeightRecords_BovineId_OccurrenceDate",
                table: "BovineRecordsWeight",
                newName: "IX_BovineRecordsWeight_BovineId_OccurrenceDate");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BovineRecordsWeight",
                table: "BovineRecordsWeight",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BovineRecordsWeight_Bovines_BovineId",
                table: "BovineRecordsWeight",
                column: "BovineId",
                principalTable: "Bovines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BovineRecordsWeight_Bovines_BovineId",
                table: "BovineRecordsWeight");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BovineRecordsWeight",
                table: "BovineRecordsWeight");

            migrationBuilder.RenameTable(
                name: "BovineRecordsWeight",
                newName: "BovineWeightRecords");

            migrationBuilder.RenameIndex(
                name: "IX_BovineRecordsWeight_BovineId_OccurrenceDate",
                table: "BovineWeightRecords",
                newName: "IX_BovineWeightRecords_BovineId_OccurrenceDate");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BovineWeightRecords",
                table: "BovineWeightRecords",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BovineWeightRecords_Bovines_BovineId",
                table: "BovineWeightRecords",
                column: "BovineId",
                principalTable: "Bovines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
