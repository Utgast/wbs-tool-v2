using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WbsTool.Api.Migrations
{
    /// <inheritdoc />
    public partial class SeedPersonCompetencies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "PersonCompetencies",
                columns: new[] { "Id", "Comment", "CompetencyId", "CreatedAtUtc", "IsActive", "PersonId", "ProficiencyLevel", "UpdatedAtUtc" },
                values: new object[,]
                {
                    { new Guid("50000000-0000-0000-0000-000000000001"), "Automatisierung und KI", new Guid("40000000-0000-0000-0000-000000000008"), new DateTime(2026, 7, 9, 0, 0, 0, 0, DateTimeKind.Utc), true, new Guid("30000000-0000-0000-0000-000000000005"), null, new DateTime(2026, 7, 9, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0000-0000-0000-000000000002"), "FM-Profil", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2026, 7, 9, 0, 0, 0, 0, DateTimeKind.Utc), true, new Guid("30000000-0000-0000-0000-000000000003"), null, new DateTime(2026, 7, 9, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0000-0000-0000-000000000003"), "CAD", new Guid("40000000-0000-0000-0000-000000000002"), new DateTime(2026, 7, 9, 0, 0, 0, 0, DateTimeKind.Utc), true, new Guid("30000000-0000-0000-0000-000000000003"), null, new DateTime(2026, 7, 9, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0000-0000-0000-000000000004"), "CAD", new Guid("40000000-0000-0000-0000-000000000002"), new DateTime(2026, 7, 9, 0, 0, 0, 0, DateTimeKind.Utc), true, new Guid("30000000-0000-0000-0000-000000000004"), null, new DateTime(2026, 7, 9, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0000-0000-0000-000000000005"), "GIS", new Guid("40000000-0000-0000-0000-000000000003"), new DateTime(2026, 7, 9, 0, 0, 0, 0, DateTimeKind.Utc), true, new Guid("30000000-0000-0000-0000-000000000004"), null, new DateTime(2026, 7, 9, 0, 0, 0, 0, DateTimeKind.Utc) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PersonCompetencies",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "PersonCompetencies",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "PersonCompetencies",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "PersonCompetencies",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "PersonCompetencies",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0000-0000-0000-000000000005"));
        }
    }
}
