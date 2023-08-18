using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecom.DataAccess.Migrations
{
    public partial class SizesTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "type",
                table: "Sizes",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "type",
                table: "Sizes");
        }
    }
}
