using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScopeLinks_LLC_Task.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addcolPayStatuInordItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6b4ab368-0bda-4b54-b743-53696393ed7b");

            migrationBuilder.AddColumn<string>(
                name: "PaymentIntentId",
                table: "OrderItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "PaymentStatus",
                table: "OrderItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FristName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "ProfilePicture", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "67c95a7b-4180-4417-810a-23178f57d87e", 0, "0b68b781-c90c-4ce0-8fb3-ccfc18e5b5c9", "adminSeed@example.com", true, "adminSeed", "adSeed", false, null, "ADMINSEED@EXAMPLE.COM", "ADMINSEED@EXAMPLE.COM", "AQAAAAIAAYagAAAAEKm0HRJv7I0coIgB1G/1PC/BorPFem948aZmhWvJMA1sF0lOIa9uKMk9ajsf3478Cg==", null, false, null, "", false, "adminSeed@example.com" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "67c95a7b-4180-4417-810a-23178f57d87e");

            migrationBuilder.DropColumn(
                name: "PaymentIntentId",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "PaymentStatus",
                table: "OrderItems");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FristName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "ProfilePicture", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "6b4ab368-0bda-4b54-b743-53696393ed7b", 0, "a4b8a8ef-8c5e-476e-a699-05ad01a96183", "adminSeed@example.com", true, "adminSeed", "adSeed", false, null, "ADMINSEED@EXAMPLE.COM", "ADMINSEED@EXAMPLE.COM", "AQAAAAIAAYagAAAAEB3o2Wk0PSGuGdWzriZU8Apmz+jkwBPQqWyUY/FjUGhAp91UdaKaVBIXJ8xZEFAKGw==", null, false, null, "", false, "adminSeed@example.com" });
        }
    }
}
