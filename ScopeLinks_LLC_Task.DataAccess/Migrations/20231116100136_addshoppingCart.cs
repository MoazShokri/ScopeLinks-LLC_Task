using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScopeLinks_LLC_Task.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addshoppingCart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6ac3c3a8-c9f1-4321-8156-833d8fdb4e07");

            migrationBuilder.AddColumn<int>(
                name: "ShoppingCartId",
                table: "CartItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "shoppingCarts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shoppingCarts", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FristName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "ProfilePicture", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "15039a24-e532-4952-a692-d30e06b3bbb4", 0, "c7d30f98-a601-4c9e-ad85-1514d73055f2", "adminSeed@example.com", true, "adminSeed", "adSeed", false, null, "ADMINSEED@EXAMPLE.COM", "ADMINSEED@EXAMPLE.COM", "AQAAAAIAAYagAAAAEFQkI4qzEFxqSoXdDRjPZKfmiEb8ocbRRmu/VmOkB+NIePSeFfgKdy67LRN+7oLioQ==", null, false, null, "", false, "adminSeed@example.com" });

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_ShoppingCartId",
                table: "CartItems",
                column: "ShoppingCartId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_shoppingCarts_ShoppingCartId",
                table: "CartItems",
                column: "ShoppingCartId",
                principalTable: "shoppingCarts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_shoppingCarts_ShoppingCartId",
                table: "CartItems");

            migrationBuilder.DropTable(
                name: "shoppingCarts");

            migrationBuilder.DropIndex(
                name: "IX_CartItems_ShoppingCartId",
                table: "CartItems");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "15039a24-e532-4952-a692-d30e06b3bbb4");

            migrationBuilder.DropColumn(
                name: "ShoppingCartId",
                table: "CartItems");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FristName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "ProfilePicture", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "6ac3c3a8-c9f1-4321-8156-833d8fdb4e07", 0, "0470b5c2-8a5e-4458-83c2-ecb71df1297b", "adminSeed@example.com", true, "adminSeed", "adSeed", false, null, "ADMINSEED@EXAMPLE.COM", "ADMINSEED@EXAMPLE.COM", "AQAAAAIAAYagAAAAEJsMjZAw1XpL5J+9B4KMtxRBPpb675ikWYn2IKEFa3l4yvFUOwbHNpNSqyV+Btqucw==", null, false, null, "", false, "adminSeed@example.com" });
        }
    }
}
