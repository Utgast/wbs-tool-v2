using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WbsTool.Api.Migrations
{
    /// <inheritdoc />
    public partial class SeedOhlCompetencies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Competencies",
                columns: new[] { "Id", "Code", "CreatedAtUtc", "Description", "IsActive", "Name", "UpdatedAtUtc" },
                values: new object[,]
                {
                    { new Guid("40000000-0000-0000-0000-000000000001"), "FM_PROFIL", new DateTime(2026, 7, 9, 0, 0, 0, 0, DateTimeKind.Utc), "Planung und Bearbeitung von Freileitungsprofilen in FM-Profil.", true, "FM-Profil", new DateTime(2026, 7, 9, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("40000000-0000-0000-0000-000000000002"), "CAD", new DateTime(2026, 7, 9, 0, 0, 0, 0, DateTimeKind.Utc), "CAD-Bearbeitung fuer Uebersichtsplaene, Lageplaene und technische Planunterlagen.", true, "CAD", new DateTime(2026, 7, 9, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("40000000-0000-0000-0000-000000000003"), "GIS", new DateTime(2026, 7, 9, 0, 0, 0, 0, DateTimeKind.Utc), "GIS-Datenverarbeitung, Datenerfassung und Aktualisierung raeumlicher Projektdaten.", true, "GIS", new DateTime(2026, 7, 9, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("40000000-0000-0000-0000-000000000004"), "STATIK", new DateTime(2026, 7, 9, 0, 0, 0, 0, DateTimeKind.Utc), "Statische Bemessung und Nachweisfuehrung fuer Freileitungsmaste und Bestandsmaste.", true, "Statik", new DateTime(2026, 7, 9, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("40000000-0000-0000-0000-000000000005"), "EMF", new DateTime(2026, 7, 9, 0, 0, 0, 0, DateTimeKind.Utc), "Berechnung elektromagnetischer Felder und Bewertung elektrischer Beeinflussung.", true, "EMF-Berechnung", new DateTime(2026, 7, 9, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("40000000-0000-0000-0000-000000000006"), "SEILBERECHNUNG", new DateTime(2026, 7, 9, 0, 0, 0, 0, DateTimeKind.Utc), "Berechnung von Seilspannungen, Durchhaengen, Passlaengen und zugehoerigen Mengenermittlungen.", true, "Seilberechnung", new DateTime(2026, 7, 9, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("40000000-0000-0000-0000-000000000007"), "PROJEKTMANAGEMENT", new DateTime(2026, 7, 9, 0, 0, 0, 0, DateTimeKind.Utc), "Projektsteuerung, Terminplanung, Ressourcenmanagement und Koordination.", true, "Projektmanagement", new DateTime(2026, 7, 9, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("40000000-0000-0000-0000-000000000008"), "AUTOMATISIERUNG", new DateTime(2026, 7, 9, 0, 0, 0, 0, DateTimeKind.Utc), "Automatisierung, KI-Unterstuetzung, Prozessentwicklung und digitale Werkzeugunterstuetzung.", true, "Automatisierung / KI", new DateTime(2026, 7, 9, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("40000000-0000-0000-0000-000000000009"), "TRASSIERUNG", new DateTime(2026, 7, 9, 0, 0, 0, 0, DateTimeKind.Utc), "Grob- und Feintrassierung von Freileitungen einschliesslich technischer und raeumlicher Randbedingungen.", true, "Trassierung", new DateTime(2026, 7, 9, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("40000000-0000-0000-0000-000000000010"), "MASTKONZEPTE", new DateTime(2026, 7, 9, 0, 0, 0, 0, DateTimeKind.Utc), "Erstellung und Bewertung von Mastkonzepten, Mastkatalogen und technischen Uebersichten.", true, "Mastkonzepte / Mastkatalog", new DateTime(2026, 7, 9, 0, 0, 0, 0, DateTimeKind.Utc) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Competencies",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Competencies",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Competencies",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "Competencies",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "Competencies",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "Competencies",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0000-0000-0000-000000000006"));

            migrationBuilder.DeleteData(
                table: "Competencies",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0000-0000-0000-000000000007"));

            migrationBuilder.DeleteData(
                table: "Competencies",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0000-0000-0000-000000000008"));

            migrationBuilder.DeleteData(
                table: "Competencies",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0000-0000-0000-000000000009"));

            migrationBuilder.DeleteData(
                table: "Competencies",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0000-0000-0000-000000000010"));
        }
    }
}
