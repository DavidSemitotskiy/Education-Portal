using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Portal.EFInfrastructure.Migrations
{
    public partial class AddIsPublishedProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder == null)
            {
                throw new Exception();
            }

            migrationBuilder.AddColumn<bool>(
                name: "IsPublished",
                table: "Courses",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder == null)
            {
                throw new Exception();
            }

            migrationBuilder.DropColumn(
                name: "IsPublished",
                table: "Courses");
        }
    }
}
