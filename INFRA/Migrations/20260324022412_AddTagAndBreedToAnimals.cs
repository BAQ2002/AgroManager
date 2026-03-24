using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace INFRA.Migrations
{
    /// <inheritdoc />
    public partial class AddTagAndBreedToAnimals : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Breed",
                table: "Swines",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Tag",
                table: "Swines",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Breed",
                table: "Bovines",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Tag",
                table: "Bovines",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Breed",
                table: "Swines");

            migrationBuilder.DropColumn(
                name: "Tag",
                table: "Swines");

            migrationBuilder.DropColumn(
                name: "Breed",
                table: "Bovines");

            migrationBuilder.DropColumn(
                name: "Tag",
                table: "Bovines");
        }
    }
}
