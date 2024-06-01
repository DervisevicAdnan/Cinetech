using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CineTech.Data.Migrations
{
    /// <inheritdoc />
    public partial class migracija123 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifikacija_Korisnik_KorisnikId",
                table: "Notifikacija");

            migrationBuilder.DropForeignKey(
                name: "FK_NotifikacijeFilma_Film_FilmId",
                table: "NotifikacijeFilma");

            migrationBuilder.DropForeignKey(
                name: "FK_NotifikacijeFilma_Notifikacija_NotifikacijaId",
                table: "NotifikacijeFilma");

            migrationBuilder.DropForeignKey(
                name: "FK_Ocjena_AspNetUsers_korisnikId",
                table: "Ocjena");

            migrationBuilder.DropForeignKey(
                name: "FK_Projekcija_Film_filmId",
                table: "Projekcija");

            migrationBuilder.DropForeignKey(
                name: "FK_Projekcija_KinoSala_kinoSalaId",
                table: "Projekcija");

            migrationBuilder.DropForeignKey(
                name: "FK_Transakcija_AspNetUsers_KorisnikId",
                table: "Transakcija");

            migrationBuilder.DropForeignKey(
                name: "FK_Transakcija_ZauzetaSjedista_ZauzetaSjedistaId",
                table: "Transakcija");

            migrationBuilder.DropForeignKey(
                name: "FK_ZanroviFilma_Film_idFilma",
                table: "ZanroviFilma");

            migrationBuilder.DropForeignKey(
                name: "FK_ZauzetaSjedista_Projekcija_ProjekcijaId",
                table: "ZauzetaSjedista");

            migrationBuilder.DropIndex(
                name: "IX_ZauzetaSjedista_ProjekcijaId",
                table: "ZauzetaSjedista");

            migrationBuilder.DropIndex(
                name: "IX_ZanroviFilma_idFilma",
                table: "ZanroviFilma");

            migrationBuilder.DropIndex(
                name: "IX_Transakcija_KorisnikId",
                table: "Transakcija");

            migrationBuilder.DropIndex(
                name: "IX_Transakcija_ZauzetaSjedistaId",
                table: "Transakcija");

            migrationBuilder.DropIndex(
                name: "IX_Projekcija_filmId",
                table: "Projekcija");

            migrationBuilder.DropIndex(
                name: "IX_Projekcija_kinoSalaId",
                table: "Projekcija");

            migrationBuilder.DropIndex(
                name: "IX_Ocjena_korisnikId",
                table: "Ocjena");

            migrationBuilder.DropIndex(
                name: "IX_NotifikacijeFilma_FilmId",
                table: "NotifikacijeFilma");

            migrationBuilder.DropIndex(
                name: "IX_NotifikacijeFilma_NotifikacijaId",
                table: "NotifikacijeFilma");

            migrationBuilder.DropIndex(
                name: "IX_Notifikacija_KorisnikId",
                table: "Notifikacija");

            migrationBuilder.RenameColumn(
                name: "ocjena",
                table: "Ocjena",
                newName: "ocjenaFilma");

            migrationBuilder.AlterColumn<string>(
                name: "KorisnikId",
                table: "Transakcija",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "korisnikId",
                table: "Ocjena",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "KorisnikId",
                table: "Notifikacija",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ocjenaFilma",
                table: "Ocjena",
                newName: "ocjena");

            migrationBuilder.AlterColumn<string>(
                name: "KorisnikId",
                table: "Transakcija",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "korisnikId",
                table: "Ocjena",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "KorisnikId",
                table: "Notifikacija",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_ZauzetaSjedista_ProjekcijaId",
                table: "ZauzetaSjedista",
                column: "ProjekcijaId");

            migrationBuilder.CreateIndex(
                name: "IX_ZanroviFilma_idFilma",
                table: "ZanroviFilma",
                column: "idFilma");

            migrationBuilder.CreateIndex(
                name: "IX_Transakcija_KorisnikId",
                table: "Transakcija",
                column: "KorisnikId");

            migrationBuilder.CreateIndex(
                name: "IX_Transakcija_ZauzetaSjedistaId",
                table: "Transakcija",
                column: "ZauzetaSjedistaId");

            migrationBuilder.CreateIndex(
                name: "IX_Projekcija_filmId",
                table: "Projekcija",
                column: "filmId");

            migrationBuilder.CreateIndex(
                name: "IX_Projekcija_kinoSalaId",
                table: "Projekcija",
                column: "kinoSalaId");

            migrationBuilder.CreateIndex(
                name: "IX_Ocjena_korisnikId",
                table: "Ocjena",
                column: "korisnikId");

            migrationBuilder.CreateIndex(
                name: "IX_NotifikacijeFilma_FilmId",
                table: "NotifikacijeFilma",
                column: "FilmId");

            migrationBuilder.CreateIndex(
                name: "IX_NotifikacijeFilma_NotifikacijaId",
                table: "NotifikacijeFilma",
                column: "NotifikacijaId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifikacija_KorisnikId",
                table: "Notifikacija",
                column: "KorisnikId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifikacija_Korisnik_KorisnikId",
                table: "Notifikacija",
                column: "KorisnikId",
                principalTable: "Korisnik",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NotifikacijeFilma_Film_FilmId",
                table: "NotifikacijeFilma",
                column: "FilmId",
                principalTable: "Film",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NotifikacijeFilma_Notifikacija_NotifikacijaId",
                table: "NotifikacijeFilma",
                column: "NotifikacijaId",
                principalTable: "Notifikacija",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ocjena_AspNetUsers_korisnikId",
                table: "Ocjena",
                column: "korisnikId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Projekcija_Film_filmId",
                table: "Projekcija",
                column: "filmId",
                principalTable: "Film",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Projekcija_KinoSala_kinoSalaId",
                table: "Projekcija",
                column: "kinoSalaId",
                principalTable: "KinoSala",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transakcija_AspNetUsers_KorisnikId",
                table: "Transakcija",
                column: "KorisnikId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transakcija_ZauzetaSjedista_ZauzetaSjedistaId",
                table: "Transakcija",
                column: "ZauzetaSjedistaId",
                principalTable: "ZauzetaSjedista",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ZanroviFilma_Film_idFilma",
                table: "ZanroviFilma",
                column: "idFilma",
                principalTable: "Film",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ZauzetaSjedista_Projekcija_ProjekcijaId",
                table: "ZauzetaSjedista",
                column: "ProjekcijaId",
                principalTable: "Projekcija",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
