using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace INFRA.Migrations
{
    /// <inheritdoc />
    public partial class AddBatchMemberForeignKeysAndIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_SwineBeefMembers_AnimalId",
                table: "SwineBeefMembers",
                column: "AnimalId",
                unique: true,
                filter: "\"BatchExitDate\" IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_SwineBeefMembers_AnimalId_BatchExitDate",
                table: "SwineBeefMembers",
                columns: new[] { "AnimalId", "BatchExitDate" });

            migrationBuilder.CreateIndex(
                name: "IX_SwineBeefMembers_BatchId",
                table: "SwineBeefMembers",
                column: "BatchId");

            migrationBuilder.CreateIndex(
                name: "IX_BovinePastureMembers_AnimalId",
                table: "BovinePastureMembers",
                column: "AnimalId",
                unique: true,
                filter: "\"BatchExitDate\" IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BovinePastureMembers_AnimalId_BatchExitDate",
                table: "BovinePastureMembers",
                columns: new[] { "AnimalId", "BatchExitDate" });

            migrationBuilder.CreateIndex(
                name: "IX_BovinePastureMembers_BatchId",
                table: "BovinePastureMembers",
                column: "BatchId");

            migrationBuilder.CreateIndex(
                name: "IX_BovinePastureBatchs_PastureId",
                table: "BovinePastureBatchs",
                column: "PastureId");

            migrationBuilder.AddForeignKey(
                name: "FK_BovinePastureBatchs_Pastures_PastureId",
                table: "BovinePastureBatchs",
                column: "PastureId",
                principalTable: "Pastures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BovinePastureMembers_BovinePastureBatchs_BatchId",
                table: "BovinePastureMembers",
                column: "BatchId",
                principalTable: "BovinePastureBatchs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BovinePastureMembers_Bovines_AnimalId",
                table: "BovinePastureMembers",
                column: "AnimalId",
                principalTable: "Bovines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SwineBeefMembers_SwineBeefBatchs_BatchId",
                table: "SwineBeefMembers",
                column: "BatchId",
                principalTable: "SwineBeefBatchs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SwineBeefMembers_Swines_AnimalId",
                table: "SwineBeefMembers",
                column: "AnimalId",
                principalTable: "Swines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BovinePastureBatchs_Pastures_PastureId",
                table: "BovinePastureBatchs");

            migrationBuilder.DropForeignKey(
                name: "FK_BovinePastureMembers_BovinePastureBatchs_BatchId",
                table: "BovinePastureMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_BovinePastureMembers_Bovines_AnimalId",
                table: "BovinePastureMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_SwineBeefMembers_SwineBeefBatchs_BatchId",
                table: "SwineBeefMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_SwineBeefMembers_Swines_AnimalId",
                table: "SwineBeefMembers");

            migrationBuilder.DropIndex(
                name: "IX_SwineBeefMembers_AnimalId",
                table: "SwineBeefMembers");

            migrationBuilder.DropIndex(
                name: "IX_SwineBeefMembers_AnimalId_BatchExitDate",
                table: "SwineBeefMembers");

            migrationBuilder.DropIndex(
                name: "IX_SwineBeefMembers_BatchId",
                table: "SwineBeefMembers");

            migrationBuilder.DropIndex(
                name: "IX_BovinePastureMembers_AnimalId",
                table: "BovinePastureMembers");

            migrationBuilder.DropIndex(
                name: "IX_BovinePastureMembers_AnimalId_BatchExitDate",
                table: "BovinePastureMembers");

            migrationBuilder.DropIndex(
                name: "IX_BovinePastureMembers_BatchId",
                table: "BovinePastureMembers");

            migrationBuilder.DropIndex(
                name: "IX_BovinePastureBatchs_PastureId",
                table: "BovinePastureBatchs");
        }
    }
}
