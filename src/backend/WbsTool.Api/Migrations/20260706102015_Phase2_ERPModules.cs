using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WbsTool.Api.Migrations
{
    /// <inheritdoc />
    public partial class Phase2_ERPModules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CapacityAllocations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    PersonId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ProjectId = table.Column<Guid>(type: "TEXT", nullable: true),
                    WbsNodeId = table.Column<Guid>(type: "TEXT", nullable: true),
                    StartDate = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    PlannedHours = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ActualHours = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AllocationPercent = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    Source = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Comment = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CapacityAllocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CapacityAllocations_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CapacityAllocations_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_CapacityAllocations_WbsNodes_WbsNodeId",
                        column: x => x.WbsNodeId,
                        principalTable: "WbsNodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Competencies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Code = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Competencies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProcessPhases",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Code = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Goal = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    Description = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                    DefaultResponsibility = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    SortOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessPhases", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoleAssignments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    PersonId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Role = table.Column<int>(type: "INTEGER", nullable: false),
                    ScopeType = table.Column<int>(type: "INTEGER", nullable: false),
                    ProjectId = table.Column<Guid>(type: "TEXT", nullable: true),
                    AssignedBy = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    AssignedAtUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ValidFrom = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    ValidUntil = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    Comment = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    RevokedBy = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    RevokedAtUtc = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleAssignments_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonCompetencies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    PersonId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CompetencyId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ProficiencyLevel = table.Column<int>(type: "INTEGER", nullable: true),
                    Comment = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonCompetencies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonCompetencies_Competencies_CompetencyId",
                        column: x => x.CompetencyId,
                        principalTable: "Competencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonCompetencies_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ResourceDemands",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ProjectId = table.Column<Guid>(type: "TEXT", nullable: false),
                    WbsNodeId = table.Column<Guid>(type: "TEXT", nullable: true),
                    RequiredCompetencyId = table.Column<Guid>(type: "TEXT", nullable: true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 300, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 4000, nullable: true),
                    PlannedHours = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    StartDate = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    EndDate = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedBy = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    StatusChangedBy = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    StatusChangedAtUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DecisionComment = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResourceDemands", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResourceDemands_Competencies_RequiredCompetencyId",
                        column: x => x.RequiredCompetencyId,
                        principalTable: "Competencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ResourceDemands_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ResourceDemands_WbsNodes_WbsNodeId",
                        column: x => x.WbsNodeId,
                        principalTable: "WbsNodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "WbsRequiredCompetencies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ProjectId = table.Column<Guid>(type: "TEXT", nullable: false),
                    WbsNodeId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CompetencyId = table.Column<Guid>(type: "TEXT", nullable: false),
                    RequiredLevel = table.Column<int>(type: "INTEGER", nullable: true),
                    Comment = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WbsRequiredCompetencies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WbsRequiredCompetencies_Competencies_CompetencyId",
                        column: x => x.CompetencyId,
                        principalTable: "Competencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WbsRequiredCompetencies_WbsNodes_WbsNodeId",
                        column: x => x.WbsNodeId,
                        principalTable: "WbsNodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WbsPhaseMappings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ProjectId = table.Column<Guid>(type: "TEXT", nullable: false),
                    WbsNodeId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ProcessPhaseId = table.Column<Guid>(type: "TEXT", nullable: false),
                    IsPrimary = table.Column<bool>(type: "INTEGER", nullable: false),
                    Comment = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WbsPhaseMappings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WbsPhaseMappings_ProcessPhases_ProcessPhaseId",
                        column: x => x.ProcessPhaseId,
                        principalTable: "ProcessPhases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WbsPhaseMappings_WbsNodes_WbsNodeId",
                        column: x => x.WbsNodeId,
                        principalTable: "WbsNodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CapacityAllocations_PersonId",
                table: "CapacityAllocations",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_CapacityAllocations_ProjectId",
                table: "CapacityAllocations",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_CapacityAllocations_WbsNodeId",
                table: "CapacityAllocations",
                column: "WbsNodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Competencies_Code",
                table: "Competencies",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PersonCompetencies_CompetencyId",
                table: "PersonCompetencies",
                column: "CompetencyId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonCompetencies_PersonId_CompetencyId",
                table: "PersonCompetencies",
                columns: new[] { "PersonId", "CompetencyId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProcessPhases_Code",
                table: "ProcessPhases",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ResourceDemands_ProjectId",
                table: "ResourceDemands",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ResourceDemands_RequiredCompetencyId",
                table: "ResourceDemands",
                column: "RequiredCompetencyId");

            migrationBuilder.CreateIndex(
                name: "IX_ResourceDemands_WbsNodeId",
                table: "ResourceDemands",
                column: "WbsNodeId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleAssignments_PersonId_Role_ScopeType_ProjectId_IsActive",
                table: "RoleAssignments",
                columns: new[] { "PersonId", "Role", "ScopeType", "ProjectId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_WbsPhaseMappings_ProcessPhaseId",
                table: "WbsPhaseMappings",
                column: "ProcessPhaseId");

            migrationBuilder.CreateIndex(
                name: "IX_WbsPhaseMappings_ProjectId_WbsNodeId_ProcessPhaseId",
                table: "WbsPhaseMappings",
                columns: new[] { "ProjectId", "WbsNodeId", "ProcessPhaseId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WbsPhaseMappings_WbsNodeId",
                table: "WbsPhaseMappings",
                column: "WbsNodeId");

            migrationBuilder.CreateIndex(
                name: "IX_WbsRequiredCompetencies_CompetencyId",
                table: "WbsRequiredCompetencies",
                column: "CompetencyId");

            migrationBuilder.CreateIndex(
                name: "IX_WbsRequiredCompetencies_ProjectId_WbsNodeId_CompetencyId",
                table: "WbsRequiredCompetencies",
                columns: new[] { "ProjectId", "WbsNodeId", "CompetencyId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WbsRequiredCompetencies_WbsNodeId",
                table: "WbsRequiredCompetencies",
                column: "WbsNodeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CapacityAllocations");

            migrationBuilder.DropTable(
                name: "PersonCompetencies");

            migrationBuilder.DropTable(
                name: "ResourceDemands");

            migrationBuilder.DropTable(
                name: "RoleAssignments");

            migrationBuilder.DropTable(
                name: "WbsPhaseMappings");

            migrationBuilder.DropTable(
                name: "WbsRequiredCompetencies");

            migrationBuilder.DropTable(
                name: "ProcessPhases");

            migrationBuilder.DropTable(
                name: "Competencies");
        }
    }
}
