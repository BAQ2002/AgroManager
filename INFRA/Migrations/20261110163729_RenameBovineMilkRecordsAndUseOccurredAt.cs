using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace INFRA.Migrations
{
    /// <inheritdoc />
    public partial class RenameBovineMilkRecordsAndUseOccurredAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BovineMilkRecords_Bovines_BovineId",
                table: "BovineMilkRecords");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BovineMilkRecords",
                table: "BovineMilkRecords");

            migrationBuilder.DropIndex(
                name: "IX_BovineMilkRecords_BovineId_OccurrenceDate",
                table: "BovineMilkRecords");

            migrationBuilder.RenameTable(
                name: "BovineMilkRecords",
                newName: "BovineRecordsMilk");

            migrationBuilder.RenameColumn(
                name: "OccurrenceDate",
                table: "BovineRecordsMilk",
                newName: "OccurredAt");

            // Use an explicit time zone so conversion does not depend on the database session.
            migrationBuilder.Sql("""
                ALTER TABLE "BovineRecordsMilk"
                ALTER COLUMN "OccurredAt" TYPE timestamp with time zone
                USING ("OccurredAt"::timestamp AT TIME ZONE 'America/Bahia');
                """);

            migrationBuilder.AddPrimaryKey(
                name: "PK_BovineRecordsMilk",
                table: "BovineRecordsMilk",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_BovineRecordsMilk_BovineId_OccurredAt",
                table: "BovineRecordsMilk",
                columns: new[] { "BovineId", "OccurredAt" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BovineRecordsMilk_Bovines_BovineId",
                table: "BovineRecordsMilk",
                column: "BovineId",
                principalTable: "Bovines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BovineRecordsMilk_Bovines_BovineId",
                table: "BovineRecordsMilk");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BovineRecordsMilk",
                table: "BovineRecordsMilk");

            migrationBuilder.DropIndex(
                name: "IX_BovineRecordsMilk_BovineId_OccurredAt",
                table: "BovineRecordsMilk");

            // Returning to a date discards the time of any measurements added after Up.
            migrationBuilder.Sql("""
                ALTER TABLE "BovineRecordsMilk"
                ALTER COLUMN "OccurredAt" TYPE date
                USING (("OccurredAt" AT TIME ZONE 'America/Bahia')::date);
                """);

            migrationBuilder.RenameColumn(
                name: "OccurredAt",
                table: "BovineRecordsMilk",
                newName: "OccurrenceDate");

            migrationBuilder.RenameTable(
                name: "BovineRecordsMilk",
                newName: "BovineMilkRecords");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BovineMilkRecords",
                table: "BovineMilkRecords",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_BovineMilkRecords_BovineId_OccurrenceDate",
                table: "BovineMilkRecords",
                columns: new[] { "BovineId", "OccurrenceDate" });

            migrationBuilder.AddForeignKey(
                name: "FK_BovineMilkRecords_Bovines_BovineId",
                table: "BovineMilkRecords",
                column: "BovineId",
                principalTable: "Bovines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
