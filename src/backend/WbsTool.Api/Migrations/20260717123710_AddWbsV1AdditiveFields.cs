using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WbsTool.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddWbsV1AdditiveFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "WbsNodes",
                type: "TEXT",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "WbsNodes",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "ProgressPercent",
                table: "WbsNodes",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ResponsiblePersonName",
                table: "WbsNodes",
                type: "TEXT",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "WbsNodes",
                type: "INTEGER",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "WbsNodes",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "WbsNodes");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "WbsNodes");

            migrationBuilder.DropColumn(
                name: "ProgressPercent",
                table: "WbsNodes");

            migrationBuilder.DropColumn(
                name: "ResponsiblePersonName",
                table: "WbsNodes");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "WbsNodes");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "WbsNodes");
        }
    }
}
