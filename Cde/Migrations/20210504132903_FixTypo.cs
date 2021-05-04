using Microsoft.EntityFrameworkCore.Migrations;

namespace Cde.Migrations
{
    public partial class FixTypo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserProject_Projects_InvitedProjecsProjectId",
                table: "ApplicationUserProject");

            migrationBuilder.RenameColumn(
                name: "InvitedProjecsProjectId",
                table: "ApplicationUserProject",
                newName: "InvitedProjectsProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_ApplicationUserProject_InvitedProjecsProjectId",
                table: "ApplicationUserProject",
                newName: "IX_ApplicationUserProject_InvitedProjectsProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserProject_Projects_InvitedProjectsProjectId",
                table: "ApplicationUserProject",
                column: "InvitedProjectsProjectId",
                principalTable: "Projects",
                principalColumn: "ProjectId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserProject_Projects_InvitedProjectsProjectId",
                table: "ApplicationUserProject");

            migrationBuilder.RenameColumn(
                name: "InvitedProjectsProjectId",
                table: "ApplicationUserProject",
                newName: "InvitedProjecsProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_ApplicationUserProject_InvitedProjectsProjectId",
                table: "ApplicationUserProject",
                newName: "IX_ApplicationUserProject_InvitedProjecsProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserProject_Projects_InvitedProjecsProjectId",
                table: "ApplicationUserProject",
                column: "InvitedProjecsProjectId",
                principalTable: "Projects",
                principalColumn: "ProjectId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
