using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScopeLinks_LLC_Task.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addcoluSorderDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "41b951ce-8057-4210-8bcf-22491879f4ad");

            migrationBuilder.AddColumn<DateTime>(
                name: "OderDate",
                table: "OrderItems",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FristName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "ProfilePicture", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "3f7616db-17ab-4511-ad73-fd5147b02dbc", 0, "a66425b0-ef82-4e97-9c8e-32954d01cee0", "adminSeed@example.com", true, "adminSeed", "adSeed", false, null, "ADMINSEED@EXAMPLE.COM", "ADMINSEED@EXAMPLE.COM", "AQAAAAIAAYagAAAAEGR8t71BVr7Ypfhe4EcYpEMgMWO3/g42BDlxEIW/AYnGEpSr2JqgiUqwygKbaIPDPw==", null, false, null, "", false, "adminSeed@example.com" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3f7616db-17ab-4511-ad73-fd5147b02dbc");

            migrationBuilder.DropColumn(
                name: "OderDate",
                table: "OrderItems");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FristName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "ProfilePicture", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "41b951ce-8057-4210-8bcf-22491879f4ad", 0, "713bcd4c-1d27-4a79-9eb9-ce9e1def3dce", "adminSeed@example.com", true, "adminSeed", "adSeed", false, null, "ADMINSEED@EXAMPLE.COM", "ADMINSEED@EXAMPLE.COM", "AQAAAAIAAYagAAAAEAXu6mU1/HCJWmHweWROIWluilqIQMMsjnkdPsqb7jFRNtHSQVfO2ohrkzFldO+Nnw==", null, false, null, "", false, "adminSeed@example.com" });
        }
    }
}
