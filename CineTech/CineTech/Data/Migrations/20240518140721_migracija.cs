using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CineTech.Data.Migrations
{
    /// <inheritdoc />
    public partial class migracija : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Film",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    naziv = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    naslovnaSlika = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    opis = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    redatelj = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    glumci = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    releseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    trailer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StatusPrikazivanja = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Film", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "KinoSala",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    naziv = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    brojRedova = table.Column<int>(type: "int", nullable: false),
                    brojKolona = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KinoSala", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Korisnik",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    imePrezime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Korisnik", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Korisnik_AspNetUsers_Id",
                        column: x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    naziv = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ZanroviFilma",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idFilma = table.Column<int>(type: "int", nullable: false),
                    zanrFilma = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZanroviFilma", x => x.id);
                    table.ForeignKey(
                        name: "FK_ZanroviFilma_Film_idFilma",
                        column: x => x.idFilma,
                        principalTable: "Film",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Projekcija",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    datum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    vrijeme = table.Column<TimeOnly>(type: "time", nullable: false),
                    cijenaOsnovneKarte = table.Column<double>(type: "float", nullable: false),
                    kinoSalaId = table.Column<int>(type: "int", nullable: false),
                    filmId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projekcija", x => x.id);
                    table.ForeignKey(
                        name: "FK_Projekcija_Film_filmId",
                        column: x => x.filmId,
                        principalTable: "Film",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Projekcija_KinoSala_kinoSalaId",
                        column: x => x.kinoSalaId,
                        principalTable: "KinoSala",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notifikacija",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KorisnikId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PeriodNotifikacije = table.Column<int>(type: "int", nullable: false),
                    StatusNotifikacije = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifikacija", x => x.id);
                    table.ForeignKey(
                        name: "FK_Notifikacija_Korisnik_KorisnikId",
                        column: x => x.KorisnikId,
                        principalTable: "Korisnik",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ocjena",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ocjena = table.Column<int>(type: "int", nullable: false),
                    komentar = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    datum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    korisnikId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ocjena", x => x.id);
                    table.ForeignKey(
                        name: "FK_Ocjena_Korisnik_korisnikId",
                        column: x => x.korisnikId,
                        principalTable: "Korisnik",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    korisnikId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    roleId = table.Column<int>(type: "int", nullable: false),
                    rolaid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.id);
                    table.ForeignKey(
                        name: "FK_UserRoles_Korisnik_korisnikId",
                        column: x => x.korisnikId,
                        principalTable: "Korisnik",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_rolaid",
                        column: x => x.rolaid,
                        principalTable: "Roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ZauzetaSjedista",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    red = table.Column<int>(type: "int", nullable: false),
                    redniBrojSjedista = table.Column<int>(type: "int", nullable: false),
                    ProjekcijaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZauzetaSjedista", x => x.id);
                    table.ForeignKey(
                        name: "FK_ZauzetaSjedista_Projekcija_ProjekcijaId",
                        column: x => x.ProjekcijaId,
                        principalTable: "Projekcija",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NotifikacijeFilma",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FilmId = table.Column<int>(type: "int", nullable: false),
                    NotifikacijaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotifikacijeFilma", x => x.id);
                    table.ForeignKey(
                        name: "FK_NotifikacijeFilma_Film_FilmId",
                        column: x => x.FilmId,
                        principalTable: "Film",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NotifikacijeFilma_Notifikacija_NotifikacijaId",
                        column: x => x.NotifikacijaId,
                        principalTable: "Notifikacija",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OcjeneFilma",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FilmId = table.Column<int>(type: "int", nullable: false),
                    OcjenaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OcjeneFilma", x => x.id);
                    table.ForeignKey(
                        name: "FK_OcjeneFilma_Film_FilmId",
                        column: x => x.FilmId,
                        principalTable: "Film",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OcjeneFilma_Ocjena_OcjenaId",
                        column: x => x.OcjenaId,
                        principalTable: "Ocjena",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transakcija",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    datum = table.Column<DateOnly>(type: "date", nullable: false),
                    vrijeme = table.Column<TimeOnly>(type: "time", nullable: false),
                    KorisnikId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ZauzetaSjedistaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transakcija", x => x.id);
                    table.ForeignKey(
                        name: "FK_Transakcija_Korisnik_KorisnikId",
                        column: x => x.KorisnikId,
                        principalTable: "Korisnik",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transakcija_ZauzetaSjedista_ZauzetaSjedistaId",
                        column: x => x.ZauzetaSjedistaId,
                        principalTable: "ZauzetaSjedista",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Kupovina",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    cijena = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kupovina", x => x.id);
                    table.ForeignKey(
                        name: "FK_Kupovina_Transakcija_id",
                        column: x => x.id,
                        principalTable: "Transakcija",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rezervacija",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rezervacija", x => x.id);
                    table.ForeignKey(
                        name: "FK_Rezervacija_Transakcija_id",
                        column: x => x.id,
                        principalTable: "Transakcija",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Notifikacija_KorisnikId",
                table: "Notifikacija",
                column: "KorisnikId");

            migrationBuilder.CreateIndex(
                name: "IX_NotifikacijeFilma_FilmId",
                table: "NotifikacijeFilma",
                column: "FilmId");

            migrationBuilder.CreateIndex(
                name: "IX_NotifikacijeFilma_NotifikacijaId",
                table: "NotifikacijeFilma",
                column: "NotifikacijaId");

            migrationBuilder.CreateIndex(
                name: "IX_Ocjena_korisnikId",
                table: "Ocjena",
                column: "korisnikId");

            migrationBuilder.CreateIndex(
                name: "IX_OcjeneFilma_FilmId",
                table: "OcjeneFilma",
                column: "FilmId");

            migrationBuilder.CreateIndex(
                name: "IX_OcjeneFilma_OcjenaId",
                table: "OcjeneFilma",
                column: "OcjenaId");

            migrationBuilder.CreateIndex(
                name: "IX_Projekcija_filmId",
                table: "Projekcija",
                column: "filmId");

            migrationBuilder.CreateIndex(
                name: "IX_Projekcija_kinoSalaId",
                table: "Projekcija",
                column: "kinoSalaId");

            migrationBuilder.CreateIndex(
                name: "IX_Transakcija_KorisnikId",
                table: "Transakcija",
                column: "KorisnikId");

            migrationBuilder.CreateIndex(
                name: "IX_Transakcija_ZauzetaSjedistaId",
                table: "Transakcija",
                column: "ZauzetaSjedistaId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_korisnikId",
                table: "UserRoles",
                column: "korisnikId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_rolaid",
                table: "UserRoles",
                column: "rolaid");

            migrationBuilder.CreateIndex(
                name: "IX_ZanroviFilma_idFilma",
                table: "ZanroviFilma",
                column: "idFilma");

            migrationBuilder.CreateIndex(
                name: "IX_ZauzetaSjedista_ProjekcijaId",
                table: "ZauzetaSjedista",
                column: "ProjekcijaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Kupovina");

            migrationBuilder.DropTable(
                name: "NotifikacijeFilma");

            migrationBuilder.DropTable(
                name: "OcjeneFilma");

            migrationBuilder.DropTable(
                name: "Rezervacija");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "ZanroviFilma");

            migrationBuilder.DropTable(
                name: "Notifikacija");

            migrationBuilder.DropTable(
                name: "Ocjena");

            migrationBuilder.DropTable(
                name: "Transakcija");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Korisnik");

            migrationBuilder.DropTable(
                name: "ZauzetaSjedista");

            migrationBuilder.DropTable(
                name: "Projekcija");

            migrationBuilder.DropTable(
                name: "Film");

            migrationBuilder.DropTable(
                name: "KinoSala");
        }
    }
}
