using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cde.Migrations
{
    public partial class CreateTimestamp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "S3Link",
                table: "Documents");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreateTimestamp",
                table: "Updates",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreateTimestamp",
                table: "Projects",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AlterColumn<byte[]>(
                name: "Blob",
                table: "Documents",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0],
                oldClrType: typeof(byte[]),
                oldType: "bytea",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreateTimestamp",
                table: "Documents",
                type: "timestamp with time zone",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateTimestamp",
                table: "Updates");

            migrationBuilder.DropColumn(
                name: "CreateTimestamp",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "CreateTimestamp",
                table: "Documents");

            migrationBuilder.AlterColumn<byte[]>(
                name: "Blob",
                table: "Documents",
                type: "bytea",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "bytea");

            migrationBuilder.AddColumn<string>(
                name: "S3Link",
                table: "Documents",
                type: "text",
                nullable: true);
        }
    }
}
