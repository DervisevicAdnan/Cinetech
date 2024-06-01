using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CineTech.Data.Migrations
{
    /// <inheritdoc />
    public partial class testicmigracija : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ocjena_Korisnik_korisnikId",
                table: "Ocjena");

            migrationBuilder.DropForeignKey(
                name: "FK_Transakcija_Korisnik_KorisnikId",
                table: "Transakcija");

            migrationBuilder.AddForeignKey(
                name: "FK_Ocjena_AspNetUsers_korisnikId",
                table: "Ocjena",
                column: "korisnikId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transakcija_AspNetUsers_KorisnikId",
                table: "Transakcija",
                column: "KorisnikId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ocjena_AspNetUsers_korisnikId",
                table: "Ocjena");

            migrationBuilder.DropForeignKey(
                name: "FK_Transakcija_AspNetUsers_KorisnikId",
                table: "Transakcija");

            migrationBuilder.AddForeignKey(
                name: "FK_Ocjena_Korisnik_korisnikId",
                table: "Ocjena",
                column: "korisnikId",
                principalTable: "Korisnik",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transakcija_Korisnik_KorisnikId",
                table: "Transakcija",
                column: "KorisnikId",
                principalTable: "Korisnik",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
