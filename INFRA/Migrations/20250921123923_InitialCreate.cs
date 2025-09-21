using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace INFRA.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BovinePastureBatchs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    Description = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    PastureId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BovinePastureBatchs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BovinePastureMembers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AnimalId = table.Column<Guid>(type: "uuid", nullable: false),
                    BatchId = table.Column<Guid>(type: "uuid", nullable: false),
                    BatchEntryDate = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    BatchExitDate = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    ExitReason = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BovinePastureMembers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Bovines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    Origin = table.Column<int>(type: "integer", nullable: false),
                    Gender = table.Column<int>(type: "integer", nullable: false),
                    BirthDate = table.Column<DateOnly>(type: "date", nullable: true),
                    PurchaseDate = table.Column<DateOnly>(type: "date", nullable: true),
                    DeathDate = table.Column<DateOnly>(type: "date", nullable: true),
                    MaritalStatus = table.Column<int>(type: "integer", nullable: true),
                    CattleType = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bovines", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pastures",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pastures", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SwineBeefBatchs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    Description = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SwineBeefBatchs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SwineBeefMembers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AnimalId = table.Column<Guid>(type: "uuid", nullable: false),
                    BatchId = table.Column<Guid>(type: "uuid", nullable: false),
                    BatchEntryDate = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    BatchExitDate = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    ExitReason = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SwineBeefMembers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Swines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    Origin = table.Column<int>(type: "integer", nullable: false),
                    Gender = table.Column<int>(type: "integer", nullable: false),
                    BirthDate = table.Column<DateOnly>(type: "date", nullable: true),
                    PurchaseDate = table.Column<DateOnly>(type: "date", nullable: true),
                    DeathDate = table.Column<DateOnly>(type: "date", nullable: true),
                    PorcType = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Swines", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BovineParentages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BreedingType = table.Column<int>(type: "integer", nullable: false),
                    FatherId = table.Column<Guid>(type: "uuid", nullable: false),
                    FatherFlag = table.Column<int>(type: "integer", nullable: false),
                    MotherId = table.Column<Guid>(type: "uuid", nullable: false),
                    SurrogateMotherId = table.Column<Guid>(type: "uuid", nullable: true),
                    MotherFlag = table.Column<int>(type: "integer", nullable: false),
                    SurrogateMotherFlag = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BovineParentages", x => x.Id);
                    table.ForeignKey(
                        name: "fk_parentage_bovine_animal",
                        column: x => x.Id,
                        principalTable: "Bovines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_parentage_bovine_father",
                        column: x => x.FatherId,
                        principalTable: "Bovines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_parentage_bovine_mother",
                        column: x => x.MotherId,
                        principalTable: "Bovines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_parentage_bovine_surrogate",
                        column: x => x.SurrogateMotherId,
                        principalTable: "Bovines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MilkRecords",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BovineId = table.Column<Guid>(type: "uuid", nullable: false),
                    OccurrenceDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Liters = table.Column<float>(type: "real", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
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

            migrationBuilder.CreateTable(
                name: "SwineParentages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BreedingType = table.Column<int>(type: "integer", nullable: false),
                    FatherId = table.Column<Guid>(type: "uuid", nullable: false),
                    FatherFlag = table.Column<int>(type: "integer", nullable: false),
                    MotherId = table.Column<Guid>(type: "uuid", nullable: false),
                    SurrogateMotherId = table.Column<Guid>(type: "uuid", nullable: true),
                    MotherFlag = table.Column<int>(type: "integer", nullable: false),
                    SurrogateMotherFlag = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SwineParentages", x => x.Id);
                    table.ForeignKey(
                        name: "fk_parentage_swine_animal",
                        column: x => x.Id,
                        principalTable: "Swines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_parentage_swine_father",
                        column: x => x.FatherId,
                        principalTable: "Swines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_parentage_swine_mother",
                        column: x => x.MotherId,
                        principalTable: "Swines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_parentage_swine_surrogate",
                        column: x => x.SurrogateMotherId,
                        principalTable: "Swines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BovineParentages_FatherId",
                table: "BovineParentages",
                column: "FatherId");

            migrationBuilder.CreateIndex(
                name: "IX_BovineParentages_MotherId",
                table: "BovineParentages",
                column: "MotherId");

            migrationBuilder.CreateIndex(
                name: "IX_BovineParentages_SurrogateMotherId",
                table: "BovineParentages",
                column: "SurrogateMotherId");

            migrationBuilder.CreateIndex(
                name: "IX_MilkRecords_BovineId",
                table: "MilkRecords",
                column: "BovineId");

            migrationBuilder.CreateIndex(
                name: "IX_SwineParentages_FatherId",
                table: "SwineParentages",
                column: "FatherId");

            migrationBuilder.CreateIndex(
                name: "IX_SwineParentages_MotherId",
                table: "SwineParentages",
                column: "MotherId");

            migrationBuilder.CreateIndex(
                name: "IX_SwineParentages_SurrogateMotherId",
                table: "SwineParentages",
                column: "SurrogateMotherId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BovineParentages");

            migrationBuilder.DropTable(
                name: "BovinePastureBatchs");

            migrationBuilder.DropTable(
                name: "BovinePastureMembers");

            migrationBuilder.DropTable(
                name: "MilkRecords");

            migrationBuilder.DropTable(
                name: "Pastures");

            migrationBuilder.DropTable(
                name: "SwineBeefBatchs");

            migrationBuilder.DropTable(
                name: "SwineBeefMembers");

            migrationBuilder.DropTable(
                name: "SwineParentages");

            migrationBuilder.DropTable(
                name: "Bovines");

            migrationBuilder.DropTable(
                name: "Swines");
        }
    }
}
