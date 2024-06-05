using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CineTech.Data.Migrations
{
    /// <inheritdoc />
    public partial class migracija415 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Korisnik_korisnikId",
                table: "UserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Roles_rolaid",
                table: "UserRoles");

            migrationBuilder.DropIndex(
                name: "IX_UserRoles_korisnikId",
                table: "UserRoles");

            migrationBuilder.DropIndex(
                name: "IX_UserRoles_rolaid",
                table: "UserRoles");

            migrationBuilder.DropColumn(
                name: "rolaid",
                table: "UserRoles");

            migrationBuilder.AlterColumn<string>(
                name: "korisnikId",
                table: "UserRoles",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "vrijeme",
                table: "Projekcija",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(TimeOnly),
                oldType: "time");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "korisnikId",
                table: "UserRoles",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "rolaid",
                table: "UserRoles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<TimeOnly>(
                name: "vrijeme",
                table: "Projekcija",
                type: "time",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_korisnikId",
                table: "UserRoles",
                column: "korisnikId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_rolaid",
                table: "UserRoles",
                column: "rolaid");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Korisnik_korisnikId",
                table: "UserRoles",
                column: "korisnikId",
                principalTable: "Korisnik",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Roles_rolaid",
                table: "UserRoles",
                column: "rolaid",
                principalTable: "Roles",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
