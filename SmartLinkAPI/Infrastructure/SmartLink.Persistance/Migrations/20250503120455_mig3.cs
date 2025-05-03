using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartLink.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class mig3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Links_Users_UserEntityId",
                table: "Links");

            migrationBuilder.RenameColumn(
                name: "UserEntityId",
                table: "Links",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Links_UserEntityId",
                table: "Links",
                newName: "IX_Links_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Links_Users_UserId",
                table: "Links",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Links_Users_UserId",
                table: "Links");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Links",
                newName: "UserEntityId");

            migrationBuilder.RenameIndex(
                name: "IX_Links_UserId",
                table: "Links",
                newName: "IX_Links_UserEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Links_Users_UserEntityId",
                table: "Links",
                column: "UserEntityId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
