using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WbsTool.Api.Migrations
{
    /// <inheritdoc />
    public partial class Phase1_ResourceAssignments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_WbsNodes_ProjectId",
                table: "WbsNodes");

            migrationBuilder.AlterColumn<decimal>(
                name: "PlannedHours",
                table: "WbsNodes",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualHours",
                table: "WbsNodes",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ImportedActualCost",
                table: "WbsNodes",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ImportedPlannedCost",
                table: "WbsNodes",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "StatusId",
                table: "WbsNodes",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    DisplayName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    ShortName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    IsPlaceholder = table.Column<bool>(type: "INTEGER", nullable: false),
                    PlaceholderType = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RateCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Code = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    HourlyRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Currency = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RateCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TaskStatuses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Code = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Label = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Color = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    SortOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsTerminal = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ResourceAssignments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    WbsNodeId = table.Column<Guid>(type: "TEXT", nullable: false),
                    PersonId = table.Column<Guid>(type: "TEXT", nullable: false),
                    AssignmentRole = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    PlannedRateCategoryId = table.Column<Guid>(type: "TEXT", nullable: true),
                    ActualRateCategoryId = table.Column<Guid>(type: "TEXT", nullable: true),
                    PlannedHours = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ActualHours = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Comment = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResourceAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResourceAssignments_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ResourceAssignments_RateCategories_ActualRateCategoryId",
                        column: x => x.ActualRateCategoryId,
                        principalTable: "RateCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ResourceAssignments_RateCategories_PlannedRateCategoryId",
                        column: x => x.PlannedRateCategoryId,
                        principalTable: "RateCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ResourceAssignments_WbsNodes_WbsNodeId",
                        column: x => x.WbsNodeId,
                        principalTable: "WbsNodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "CreatedAtUtc", "DisplayName", "Email", "IsActive", "IsPlaceholder", "PlaceholderType", "ShortName", "UpdatedAtUtc" },
                values: new object[,]
                {
                    { new Guid("30000000-0000-0000-0000-000000000001"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "alle", null, true, true, "All", "alle", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("30000000-0000-0000-0000-000000000002"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ingenieur", null, true, true, "RolePlaceholder", "Ingenieur", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("30000000-0000-0000-0000-000000000003"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Tobias", "tobias@example.local", true, false, null, "Tobias", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("30000000-0000-0000-0000-000000000004"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ibrahim", "ibrahim@example.local", true, false, null, "Ibrahim", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("30000000-0000-0000-0000-000000000005"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Dennis", "dennis@example.local", true, false, null, "Dennis", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "RateCategories",
                columns: new[] { "Id", "Code", "CreatedAtUtc", "Currency", "HourlyRate", "IsActive", "Name", "UpdatedAtUtc" },
                values: new object[,]
                {
                    { new Guid("20000000-0000-0000-0000-000000000001"), "A", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "EUR", 120m, true, "Kategorie A", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("20000000-0000-0000-0000-000000000002"), "B", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "EUR", 95m, true, "Kategorie B", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("20000000-0000-0000-0000-000000000003"), "C", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "EUR", 75m, true, "Kategorie C", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "TaskStatuses",
                columns: new[] { "Id", "Code", "Color", "IsActive", "IsTerminal", "Label", "SortOrder" },
                values: new object[,]
                {
                    { new Guid("10000000-0000-0000-0000-000000000001"), "Empty", "#94A3B8", true, false, "leer", 10 },
                    { new Guid("10000000-0000-0000-0000-000000000002"), "InCreation", "#F59E0B", true, false, "in Erstellung", 20 },
                    { new Guid("10000000-0000-0000-0000-000000000003"), "Delivered", "#3B82F6", true, false, "geliefert", 30 },
                    { new Guid("10000000-0000-0000-0000-000000000004"), "Blocked", "#DC2626", true, false, "blockiert", 40 },
                    { new Guid("10000000-0000-0000-0000-000000000005"), "Done", "#16A34A", true, true, "abgeschlossen", 50 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_WbsNodes_ProjectId_VisibleWbsId",
                table: "WbsNodes",
                columns: new[] { "ProjectId", "VisibleWbsId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WbsNodes_StatusId",
                table: "WbsNodes",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_RateCategories_Code",
                table: "RateCategories",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ResourceAssignments_ActualRateCategoryId",
                table: "ResourceAssignments",
                column: "ActualRateCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ResourceAssignments_PersonId",
                table: "ResourceAssignments",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_ResourceAssignments_PlannedRateCategoryId",
                table: "ResourceAssignments",
                column: "PlannedRateCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ResourceAssignments_WbsNodeId_PersonId_AssignmentRole_IsActive",
                table: "ResourceAssignments",
                columns: new[] { "WbsNodeId", "PersonId", "AssignmentRole", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_TaskStatuses_Code",
                table: "TaskStatuses",
                column: "Code",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_WbsNodes_TaskStatuses_StatusId",
                table: "WbsNodes",
                column: "StatusId",
                principalTable: "TaskStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WbsNodes_TaskStatuses_StatusId",
                table: "WbsNodes");

            migrationBuilder.DropTable(
                name: "ResourceAssignments");

            migrationBuilder.DropTable(
                name: "TaskStatuses");

            migrationBuilder.DropTable(
                name: "Persons");

            migrationBuilder.DropTable(
                name: "RateCategories");

            migrationBuilder.DropIndex(
                name: "IX_WbsNodes_ProjectId_VisibleWbsId",
                table: "WbsNodes");

            migrationBuilder.DropIndex(
                name: "IX_WbsNodes_StatusId",
                table: "WbsNodes");

            migrationBuilder.DropColumn(
                name: "ImportedActualCost",
                table: "WbsNodes");

            migrationBuilder.DropColumn(
                name: "ImportedPlannedCost",
                table: "WbsNodes");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "WbsNodes");

            migrationBuilder.AlterColumn<decimal>(
                name: "PlannedHours",
                table: "WbsNodes",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualHours",
                table: "WbsNodes",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WbsNodes_ProjectId",
                table: "WbsNodes",
                column: "ProjectId");
        }
    }
}
