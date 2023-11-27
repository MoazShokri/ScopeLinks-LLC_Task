using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScopeLinks_LLC_Task.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addColOrderDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "93cd073d-86cc-43ae-830e-655ee3d89f89");

            migrationBuilder.AddColumn<DateTime>(
                name: "OrderDate",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FristName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "ProfilePicture", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "6ac3c3a8-c9f1-4321-8156-833d8fdb4e07", 0, "0470b5c2-8a5e-4458-83c2-ecb71df1297b", "adminSeed@example.com", true, "adminSeed", "adSeed", false, null, "ADMINSEED@EXAMPLE.COM", "ADMINSEED@EXAMPLE.COM", "AQAAAAIAAYagAAAAEJsMjZAw1XpL5J+9B4KMtxRBPpb675ikWYn2IKEFa3l4yvFUOwbHNpNSqyV+Btqucw==", null, false, null, "", false, "adminSeed@example.com" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6ac3c3a8-c9f1-4321-8156-833d8fdb4e07");

            migrationBuilder.DropColumn(
                name: "OrderDate",
                table: "Orders");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FristName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "ProfilePicture", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "93cd073d-86cc-43ae-830e-655ee3d89f89", 0, "2d164b05-42e0-4cd6-91bc-336aca127f60", "adminSeed@example.com", true, "adminSeed", "adSeed", false, null, "ADMINSEED@EXAMPLE.COM", "ADMINSEED@EXAMPLE.COM", "AQAAAAIAAYagAAAAEKUXVKJsvrRYqRum4efuwdydUKN3ldTrLrMZo9/5kRBjrpmRoeQda4A/GQGvHKbP3w==", null, false, null, "", false, "adminSeed@example.com" });
        }
    }
}
