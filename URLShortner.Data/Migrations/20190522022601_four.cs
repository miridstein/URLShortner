using Microsoft.EntityFrameworkCore.Migrations;

namespace URLShortner.Data.Migrations
{
    public partial class four : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_UrlLinks_UrlLinkId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_UrlLinkId",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "UrlLinks",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UrlLinks");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UrlLinkId",
                table: "Users",
                column: "UrlLinkId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_UrlLinks_UrlLinkId",
                table: "Users",
                column: "UrlLinkId",
                principalTable: "UrlLinks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
