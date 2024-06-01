using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagementSystemApplication.Migrations
{
    public partial class Fourth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "age",
                table: "Authors");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "age",
                table: "Authors",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
