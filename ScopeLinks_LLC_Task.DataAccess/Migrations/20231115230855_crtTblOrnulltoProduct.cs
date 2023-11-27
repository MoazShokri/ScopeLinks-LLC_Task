using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScopeLinks_LLC_Task.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class crtTblOrnulltoProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "68c91c22-b4c4-48e4-b1a0-7cfba7f5e14f");

            migrationBuilder.AlterColumn<string>(
                name: "ProductName",
                table: "CartItems",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FristName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "ProfilePicture", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "6b4ab368-0bda-4b54-b743-53696393ed7b", 0, "a4b8a8ef-8c5e-476e-a699-05ad01a96183", "adminSeed@example.com", true, "adminSeed", "adSeed", false, null, "ADMINSEED@EXAMPLE.COM", "ADMINSEED@EXAMPLE.COM", "AQAAAAIAAYagAAAAEB3o2Wk0PSGuGdWzriZU8Apmz+jkwBPQqWyUY/FjUGhAp91UdaKaVBIXJ8xZEFAKGw==", null, false, null, "", false, "adminSeed@example.com" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6b4ab368-0bda-4b54-b743-53696393ed7b");

            migrationBuilder.AlterColumn<string>(
                name: "ProductName",
                table: "CartItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FristName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "ProfilePicture", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "68c91c22-b4c4-48e4-b1a0-7cfba7f5e14f", 0, "c271825f-7785-4d32-8e3f-3f77239a235d", "adminSeed@example.com", true, "adminSeed", "adSeed", false, null, "ADMINSEED@EXAMPLE.COM", "ADMINSEED@EXAMPLE.COM", "AQAAAAIAAYagAAAAEGYDMU58885ovuSGdTOG0VIldoV+CqOZTp6Y53ZbJrNo6rk9cnpESMPsg4sD8lbFrQ==", null, false, null, "", false, "adminSeed@example.com" });
        }
    }
}
