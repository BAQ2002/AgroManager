using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace INFRA.Migrations
{
    /// <inheritdoc />
    public partial class AddSwinweFkAndIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_SwineWeightRecords_SwineId_OccurrenceDate",
                table: "SwineWeightRecords",
                columns: new[] { "SwineId", "OccurrenceDate" });

            migrationBuilder.AddForeignKey(
                name: "FK_SwineWeightRecords_Swines_SwineId",
                table: "SwineWeightRecords",
                column: "SwineId",
                principalTable: "Swines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SwineWeightRecords_Swines_SwineId",
                table: "SwineWeightRecords");

            migrationBuilder.DropIndex(
                name: "IX_SwineWeightRecords_SwineId_OccurrenceDate",
                table: "SwineWeightRecords");
        }
    }
}
