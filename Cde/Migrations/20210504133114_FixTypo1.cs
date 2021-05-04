using Microsoft.EntityFrameworkCore.Migrations;

namespace Cde.Migrations
{
    public partial class FixTypo1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserProject_AspNetUsers_InvitedPatricipantsId",
                table: "ApplicationUserProject");

            migrationBuilder.RenameColumn(
                name: "InvitedPatricipantsId",
                table: "ApplicationUserProject",
                newName: "InvitedParticipantsId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserProject_AspNetUsers_InvitedParticipantsId",
                table: "ApplicationUserProject",
                column: "InvitedParticipantsId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserProject_AspNetUsers_InvitedParticipantsId",
                table: "ApplicationUserProject");

            migrationBuilder.RenameColumn(
                name: "InvitedParticipantsId",
                table: "ApplicationUserProject",
                newName: "InvitedPatricipantsId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserProject_AspNetUsers_InvitedPatricipantsId",
                table: "ApplicationUserProject",
                column: "InvitedPatricipantsId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
