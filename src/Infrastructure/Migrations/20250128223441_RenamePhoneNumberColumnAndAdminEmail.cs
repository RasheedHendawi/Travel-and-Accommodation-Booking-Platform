using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenamePhoneNumberColumnAndAdminEmail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PhoneNubmer",
                table: "Owners",
                newName: "PhoneNumber");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("f403d2ed-7f0d-499c-a6e9-7e3d751f842a"),
                column: "Email",
                value: "Admin@hotelmanager.com");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "Owners",
                newName: "PhoneNubmer");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("f403d2ed-7f0d-499c-a6e9-7e3d751f842a"),
                column: "Email",
                value: "Admin@hotelManager.com");
        }
    }
}
