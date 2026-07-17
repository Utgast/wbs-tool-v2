using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WbsTool.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddCompetenciesAndCapacityV1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
CREATE TABLE IF NOT EXISTS ""Competencies"" (
    ""Id"" TEXT NOT NULL CONSTRAINT ""PK_Competencies"" PRIMARY KEY,
    ""Name"" TEXT NOT NULL
);");

            migrationBuilder.Sql(@"
CREATE TABLE IF NOT EXISTS ""Persons"" (
    ""Id"" TEXT NOT NULL CONSTRAINT ""PK_Persons"" PRIMARY KEY,
    ""Name"" TEXT NOT NULL
);");

            migrationBuilder.CreateTable(
                name: "PersonAllocations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    PersonId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ProjectId = table.Column<Guid>(type: "TEXT", nullable: false),
                    WbsNodeId = table.Column<Guid>(type: "TEXT", nullable: true),
                    WeekStartDate = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    PlannedHours = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonAllocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonAllocations_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonAllocations_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonAllocations_WbsNodes_WbsNodeId",
                        column: x => x.WbsNodeId,
                        principalTable: "WbsNodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "PersonCapacities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    PersonId = table.Column<Guid>(type: "TEXT", nullable: false),
                    WeekStartDate = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    AvailableHours = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonCapacities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonCapacities_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.Sql(@"
CREATE TABLE IF NOT EXISTS ""PersonCompetencies"" (
    ""Id"" TEXT NOT NULL CONSTRAINT ""PK_PersonCompetencies"" PRIMARY KEY,
    ""PersonId"" TEXT NOT NULL,
    ""CompetencyId"" TEXT NOT NULL,
    CONSTRAINT ""FK_PersonCompetencies_Competencies_CompetencyId"" FOREIGN KEY (""CompetencyId"") REFERENCES ""Competencies"" (""Id"") ON DELETE CASCADE,
    CONSTRAINT ""FK_PersonCompetencies_Persons_PersonId"" FOREIGN KEY (""PersonId"") REFERENCES ""Persons"" (""Id"") ON DELETE CASCADE
);");

            migrationBuilder.CreateIndex(
                name: "IX_PersonAllocations_PersonId",
                table: "PersonAllocations",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonAllocations_ProjectId",
                table: "PersonAllocations",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonAllocations_WbsNodeId",
                table: "PersonAllocations",
                column: "WbsNodeId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonCapacities_PersonId_WeekStartDate",
                table: "PersonCapacities",
                columns: new[] { "PersonId", "WeekStartDate" },
                unique: true);

            migrationBuilder.Sql(@"CREATE INDEX IF NOT EXISTS ""IX_PersonCompetencies_CompetencyId"" ON ""PersonCompetencies"" (""CompetencyId"");");

            migrationBuilder.Sql(@"CREATE UNIQUE INDEX IF NOT EXISTS ""IX_PersonCompetencies_PersonId_CompetencyId"" ON ""PersonCompetencies"" (""PersonId"", ""CompetencyId"");");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PersonAllocations");

            migrationBuilder.DropTable(
                name: "PersonCapacities");
        }
    }
}
