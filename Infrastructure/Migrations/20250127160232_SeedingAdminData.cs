using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedingAdminData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "FirstName", "LastName", "Password" },
                values: new object[] { new Guid("f403d2ed-7f0d-499c-a6e9-7e3d751f842a"), "Admin@hotelManager.com", "Super", "Admin", "AQAAAAIAAYagAAAAEMcAhymzoRbYY1s8WP2AWcrQV3CHk35ny+1XHcuYxyfVqKIy5IaRVHzHa4SqBJOzFQ==" });

            migrationBuilder.InsertData(
                table: "UserRole",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("c23401af-cbb8-4b73-8d6f-ecbb5e31d3b7"), new Guid("f403d2ed-7f0d-499c-a6e9-7e3d751f842a") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserRole",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("c23401af-cbb8-4b73-8d6f-ecbb5e31d3b7"), new Guid("f403d2ed-7f0d-499c-a6e9-7e3d751f842a") });

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("f403d2ed-7f0d-499c-a6e9-7e3d751f842a"));
        }
    }
}
