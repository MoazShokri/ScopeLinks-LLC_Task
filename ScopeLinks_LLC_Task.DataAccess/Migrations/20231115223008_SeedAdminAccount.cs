using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScopeLinks_LLC_Task.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class SeedAdminAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FristName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "ProfilePicture", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "c4c5ce44-213a-412e-97c9-957f46233d8f", 0, "b734c813-aba1-4464-848f-217740bb2a5a", "adminSeed@example.com", true, "adminSeed", "adSeed", false, null, "ADMINSEED@EXAMPLE.COM", "ADMINSEED@EXAMPLE.COM", "AQAAAAIAAYagAAAAEFP151oni6jiYEFuY3p/A/b3+r7H8PvP/+joO6KRx4daPC7XRFmWG8dc89NAxYHPVA==", null, false, null, "", false, "adminSeed@example.com" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c4c5ce44-213a-412e-97c9-957f46233d8f");
        }
    }
}
