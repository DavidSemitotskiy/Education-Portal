using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Portal.EFInfrastructure.Migrations
{
    public partial class AddCourseStateMaterialStateAndChangeReferences : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder == null)
            {
                throw new Exception();
            }

            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Users_OwnerUserId",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_Courses_OwnerUserId",
                table: "Courses");

            migrationBuilder.RenameColumn(
                name: "OwnerUserId",
                table: "Courses",
                newName: "OwnerUser");

            migrationBuilder.CreateTable(
                name: "CourseStates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsFinished = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseStates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MaterialStates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OwnerMaterial = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialStates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CourseStateMaterialState",
                columns: table => new
                {
                    CourseStatesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaterialStatesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseStateMaterialState", x => new { x.CourseStatesId, x.MaterialStatesId });
                    table.ForeignKey(
                        name: "FK_CourseStateMaterialState_CourseStates_CourseStatesId",
                        column: x => x.CourseStatesId,
                        principalTable: "CourseStates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseStateMaterialState_MaterialStates_MaterialStatesId",
                        column: x => x.MaterialStatesId,
                        principalTable: "MaterialStates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseStateMaterialState_MaterialStatesId",
                table: "CourseStateMaterialState",
                column: "MaterialStatesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder == null)
            {
                throw new Exception();
            }

            migrationBuilder.DropTable(
                name: "CourseStateMaterialState");

            migrationBuilder.DropTable(
                name: "CourseStates");

            migrationBuilder.DropTable(
                name: "MaterialStates");

            migrationBuilder.RenameColumn(
                name: "OwnerUser",
                table: "Courses",
                newName: "OwnerUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_OwnerUserId",
                table: "Courses",
                column: "OwnerUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Users_OwnerUserId",
                table: "Courses",
                column: "OwnerUserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
