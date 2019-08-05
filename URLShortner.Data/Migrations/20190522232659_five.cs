using Microsoft.EntityFrameworkCore.Migrations;

namespace URLShortner.Data.Migrations
{
    public partial class five : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "UrlLinks",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "Views",
                table: "UrlLinks",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Views",
                table: "UrlLinks");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "UrlLinks",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
