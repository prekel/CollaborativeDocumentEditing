using Microsoft.EntityFrameworkCore.Migrations;

namespace Cde.Migrations
{
    public partial class InvitedPatricipants : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicationUserProject",
                columns: table => new
                {
                    InvitedPatricipantsId = table.Column<string>(type: "text", nullable: false),
                    InvitedProjecsProjectId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserProject", x => new { x.InvitedPatricipantsId, x.InvitedProjecsProjectId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserProject_AspNetUsers_InvitedPatricipantsId",
                        column: x => x.InvitedPatricipantsId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserProject_Projects_InvitedProjecsProjectId",
                        column: x => x.InvitedProjecsProjectId,
                        principalTable: "Projects",
                        principalColumn: "ProjectId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserProject_InvitedProjecsProjectId",
                table: "ApplicationUserProject",
                column: "InvitedProjecsProjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserProject");
        }
    }
}
