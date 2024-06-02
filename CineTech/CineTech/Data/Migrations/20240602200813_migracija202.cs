using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CineTech.Data.Migrations
{
    /// <inheritdoc />
    public partial class migracija202 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OcjeneFilma_Film_FilmId",
                table: "OcjeneFilma");

            migrationBuilder.DropForeignKey(
                name: "FK_OcjeneFilma_Ocjena_OcjenaId",
                table: "OcjeneFilma");

            migrationBuilder.DropIndex(
                name: "IX_OcjeneFilma_FilmId",
                table: "OcjeneFilma");

            migrationBuilder.DropIndex(
                name: "IX_OcjeneFilma_OcjenaId",
                table: "OcjeneFilma");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_OcjeneFilma_FilmId",
                table: "OcjeneFilma",
                column: "FilmId");

            migrationBuilder.CreateIndex(
                name: "IX_OcjeneFilma_OcjenaId",
                table: "OcjeneFilma",
                column: "OcjenaId");

            migrationBuilder.AddForeignKey(
                name: "FK_OcjeneFilma_Film_FilmId",
                table: "OcjeneFilma",
                column: "FilmId",
                principalTable: "Film",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OcjeneFilma_Ocjena_OcjenaId",
                table: "OcjeneFilma",
                column: "OcjenaId",
                principalTable: "Ocjena",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
