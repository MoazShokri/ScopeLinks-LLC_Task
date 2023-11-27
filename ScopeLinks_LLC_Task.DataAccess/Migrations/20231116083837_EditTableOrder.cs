using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScopeLinks_LLC_Task.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class EditTableOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "837cfdb4-b62b-47e7-a6ab-16d1c076dc96");

            migrationBuilder.DropColumn(
                name: "OrderDate",
                table: "Orders");

            migrationBuilder.AddColumn<int>(
                name: "PaymentStatus",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Orders",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FristName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "ProfilePicture", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "93cd073d-86cc-43ae-830e-655ee3d89f89", 0, "2d164b05-42e0-4cd6-91bc-336aca127f60", "adminSeed@example.com", true, "adminSeed", "adSeed", false, null, "ADMINSEED@EXAMPLE.COM", "ADMINSEED@EXAMPLE.COM", "AQAAAAIAAYagAAAAEKUXVKJsvrRYqRum4efuwdydUKN3ldTrLrMZo9/5kRBjrpmRoeQda4A/GQGvHKbP3w==", null, false, null, "", false, "adminSeed@example.com" });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AspNetUsers_UserId",
                table: "Orders",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_UserId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_UserId",
                table: "Orders");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "93cd073d-86cc-43ae-830e-655ee3d89f89");

            migrationBuilder.DropColumn(
                name: "PaymentStatus",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Orders");

            migrationBuilder.AddColumn<DateTime>(
                name: "OrderDate",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FristName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "ProfilePicture", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "837cfdb4-b62b-47e7-a6ab-16d1c076dc96", 0, "fd760de0-b3a8-4aba-81c0-52500ab811d3", "adminSeed@example.com", true, "adminSeed", "adSeed", false, null, "ADMINSEED@EXAMPLE.COM", "ADMINSEED@EXAMPLE.COM", "AQAAAAIAAYagAAAAEBDbtBXN9vmFmj+Fzo5mk5Uq3rD99KEKIwaXnsV+N/68YeI3ugbF+gsjybIIdigHJA==", null, false, null, "", false, "adminSeed@example.com" });
        }
    }
}
