using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CineTech.Data.Migrations
{
    /// <inheritdoc />
    public partial class migracija862 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ZauzetaSjedistaId",
                table: "Transakcija");

            migrationBuilder.AddColumn<int>(
                name: "TransakcijaId",
                table: "ZauzetaSjedista",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransakcijaId",
                table: "ZauzetaSjedista");

            migrationBuilder.AddColumn<int>(
                name: "ZauzetaSjedistaId",
                table: "Transakcija",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
