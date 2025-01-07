using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TicketingTool.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.UniqueConstraint("AK_AspNetUsers_UserName", x => x.UserName);
                });

            migrationBuilder.CreateTable(
                name: "Project",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Counter = table.Column<int>(type: "int", nullable: false),
                    ProjectName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Status",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StatusName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Status", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUserProject",
                columns: table => new
                {
                    ProjectsID = table.Column<int>(type: "int", nullable: false),
                    UsersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserProject", x => new { x.ProjectsID, x.UsersId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserProject_AspNetUsers_UsersId",
                        column: x => x.UsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserProject_Project_ProjectsID",
                        column: x => x.ProjectsID,
                        principalTable: "Project",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Component",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ComponentName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProjectID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Component", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Component_Project_ProjectID",
                        column: x => x.ProjectID,
                        principalTable: "Project",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "TicketField",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectID = table.Column<int>(type: "int", nullable: false),
                    FieldName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketField", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TicketField_Project_ProjectID",
                        column: x => x.ProjectID,
                        principalTable: "Project",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Component2Project",
                columns: table => new
                {
                    ComponentID = table.Column<int>(type: "int", nullable: false),
                    ProjectID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Component2Project", x => new { x.ComponentID, x.ProjectID });
                    table.ForeignKey(
                        name: "FK_Component2Project_Component_ComponentID",
                        column: x => x.ComponentID,
                        principalTable: "Component",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Component2Project_Project_ProjectID",
                        column: x => x.ProjectID,
                        principalTable: "Project",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ticket",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IssueKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProjectID = table.Column<int>(type: "int", nullable: false),
                    ComponentID = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    StatusID = table.Column<int>(type: "int", nullable: false),
                    CreatorID = table.Column<string>(type: "nvarchar(256)", nullable: false),
                    AssigneeID = table.Column<string>(type: "nvarchar(256)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ResolvedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ticket", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Ticket_AspNetUsers_AssigneeID",
                        column: x => x.AssigneeID,
                        principalTable: "AspNetUsers",
                        principalColumn: "UserName",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ticket_AspNetUsers_CreatorID",
                        column: x => x.CreatorID,
                        principalTable: "AspNetUsers",
                        principalColumn: "UserName",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ticket_Component_ComponentID",
                        column: x => x.ComponentID,
                        principalTable: "Component",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ticket_Project_ProjectID",
                        column: x => x.ProjectID,
                        principalTable: "Project",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ticket_Status_StatusID",
                        column: x => x.StatusID,
                        principalTable: "Status",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TicketChange",
                columns: table => new
                {
                    ChangeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TicketID = table.Column<int>(type: "int", nullable: false),
                    ChangedFieldID = table.Column<int>(type: "int", nullable: false),
                    ChangedFieldRefID = table.Column<int>(type: "int", nullable: false),
                    OldValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NewValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    changedByID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChangedByRefId = table.Column<int>(type: "int", nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketChange", x => x.ChangeID);
                    table.ForeignKey(
                        name: "FK_TicketChange_AspNetUsers_ChangedByRefId",
                        column: x => x.ChangedByRefId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TicketChange_TicketField_ChangedFieldRefID",
                        column: x => x.ChangedFieldRefID,
                        principalTable: "TicketField",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TicketChange_Ticket_TicketID",
                        column: x => x.TicketID,
                        principalTable: "Ticket",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { 1, null, "Admin", "ADMIN" },
                    { 2, null, "Project Manager", "MANAGER" },
                    { 3, null, "User", "USER" },
                    { 4, null, "Technical User", "TECH" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "Surname", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { 1, 0, "d7de44f6-34db-4b0e-a6ed-048a00cc1bef", null, false, false, null, "Admin", null, "X001", "AQAAAAIAAYagAAAAEE5TN6ejW4HqoBHt3bBQK94O2M3+OhuzcsnfXXNdVAD1A/TLGR6sFY4EXYcMOzz0zA==", null, false, "b57cf767-f75e-485e-a6d3-b807918dc29b", "User", false, "X001" },
                    { 2, 0, "c0270ec9-c69b-4fe0-aa5c-7261b44f8782", null, false, false, null, "Ticket", null, "TECH01", "AQAAAAIAAYagAAAAEHy4MWpQRoIXG+jXqrMzlYUcNi6nV0nkuTcJvdI87bi/EbkQg83mANLt69b9TjBIHw==", null, false, "733783ae-88c2-41bb-a5b4-7110c6a18d0f", "Creator", false, "TECH01" },
                    { 3, 0, "788b2601-2a90-422f-a354-23c997cb99d1", null, false, false, null, "Job", null, "TECH02", "AQAAAAIAAYagAAAAEBqZjFd3rs/0BY16tbCEFDf/HziB3oFUdOlWc/QGx1agOaQfISRD1cFjLC71ZgdIDg==", null, false, "2b76c678-d805-4725-b6d3-71257a03c974", "User", false, "TECH02" }
                });

            migrationBuilder.InsertData(
                table: "Component",
                columns: new[] { "ID", "ComponentName", "ProjectID" },
                values: new object[,]
                {
                    { 1, "Unidentified", null },
                    { 2, "User Interface Module", null },
                    { 3, "Database Management", null },
                    { 4, "API Gateway", null },
                    { 5, "Logging Service", null },
                    { 6, "Notification System", null },
                    { 7, "Payment Processor", null },
                    { 8, "Analytics Engine", null },
                    { 9, "Reporting Tool", null },
                    { 10, "Cache Management", null },
                    { 11, "Authentication Service", null }
                });

            migrationBuilder.InsertData(
                table: "Project",
                columns: new[] { "ID", "Counter", "ProjectKey", "ProjectName" },
                values: new object[] { 1, 5, "BSC", "Basic Project" });

            migrationBuilder.InsertData(
                table: "Status",
                columns: new[] { "ID", "StatusName" },
                values: new object[,]
                {
                    { 1, "Open" },
                    { 2, "In Progress" },
                    { 3, "Resolved" },
                    { 4, "Closed" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 4, 2 },
                    { 4, 3 }
                });

            migrationBuilder.InsertData(
                table: "Component2Project",
                columns: new[] { "ComponentID", "ProjectID" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 1 },
                    { 3, 1 },
                    { 4, 1 },
                    { 5, 1 },
                    { 6, 1 },
                    { 7, 1 },
                    { 8, 1 },
                    { 9, 1 },
                    { 10, 1 },
                    { 11, 1 }
                });

            migrationBuilder.InsertData(
                table: "Ticket",
                columns: new[] { "ID", "AssigneeID", "ComponentID", "CreatedDate", "CreatorID", "Description", "IssueKey", "LastUpdatedDate", "ProjectID", "ResolvedDate", "StatusID", "Title" },
                values: new object[,]
                {
                    { 1, null, 1, new DateTime(2025, 1, 5, 20, 57, 6, 335, DateTimeKind.Local).AddTicks(3359), "X001", "Lorem ipsum odor amet, consectetuer adipiscing elit. Curabitur duis non dis ligula potenti praesent aenean. Mus etiam ridiculus viverra sed sapien nascetur, turpis tempor sollicitudin. Aptent enim luctus dui; urna per id. Sodales auctor vel accumsan dictumst placerat feugiat lectus curabitur? Quam risus lorem vitae commodo porttitor orci ultrices.", "BSC-1", new DateTime(2025, 1, 5, 20, 57, 6, 336, DateTimeKind.Local).AddTicks(9648), 1, null, 1, "Seed Ticket 1" },
                    { 2, null, 1, new DateTime(2025, 1, 5, 20, 57, 6, 336, DateTimeKind.Local).AddTicks(9857), "X001", "Lorem ipsum odor amet, consectetuer adipiscing elit. Curabitur duis non dis ligula potenti praesent aenean. Mus etiam ridiculus viverra sed sapien nascetur, turpis tempor sollicitudin. Aptent enim luctus dui; urna per id. Sodales auctor vel accumsan dictumst placerat feugiat lectus curabitur? Quam risus lorem vitae commodo porttitor orci ultrices.", "BSC-2", new DateTime(2025, 1, 5, 20, 57, 6, 336, DateTimeKind.Local).AddTicks(9861), 1, null, 1, "Seed Ticket 2" },
                    { 3, null, 1, new DateTime(2025, 1, 5, 20, 57, 6, 336, DateTimeKind.Local).AddTicks(9864), "X001", "Lorem ipsum odor amet, consectetuer adipiscing elit. Curabitur duis non dis ligula potenti praesent aenean. Mus etiam ridiculus viverra sed sapien nascetur, turpis tempor sollicitudin. Aptent enim luctus dui; urna per id. Sodales auctor vel accumsan dictumst placerat feugiat lectus curabitur? Quam risus lorem vitae commodo porttitor orci ultrices.", "BSC-3", new DateTime(2025, 1, 5, 20, 57, 6, 336, DateTimeKind.Local).AddTicks(9865), 1, null, 1, "Seed Ticket 3" },
                    { 4, null, 1, new DateTime(2025, 1, 5, 20, 57, 6, 336, DateTimeKind.Local).AddTicks(9927), "X001", "Lorem ipsum odor amet, consectetuer adipiscing elit. Curabitur duis non dis ligula potenti praesent aenean. Mus etiam ridiculus viverra sed sapien nascetur, turpis tempor sollicitudin. Aptent enim luctus dui; urna per id. Sodales auctor vel accumsan dictumst placerat feugiat lectus curabitur? Quam risus lorem vitae commodo porttitor orci ultrices.", "BSC-4", new DateTime(2025, 1, 5, 20, 57, 6, 336, DateTimeKind.Local).AddTicks(9928), 1, null, 1, "Seed Ticket 4" },
                    { 5, null, 1, new DateTime(2025, 1, 5, 20, 57, 6, 336, DateTimeKind.Local).AddTicks(9930), "X001", "Lorem ipsum odor amet, consectetuer adipiscing elit. Curabitur duis non dis ligula potenti praesent aenean. Mus etiam ridiculus viverra sed sapien nascetur, turpis tempor sollicitudin. Aptent enim luctus dui; urna per id. Sodales auctor vel accumsan dictumst placerat feugiat lectus curabitur? Quam risus lorem vitae commodo porttitor orci ultrices.", "BSC-5", new DateTime(2025, 1, 5, 20, 57, 6, 336, DateTimeKind.Local).AddTicks(9932), 1, null, 1, "Seed Ticket 5" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserProject_UsersId",
                table: "ApplicationUserProject",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Component_ProjectID",
                table: "Component",
                column: "ProjectID");

            migrationBuilder.CreateIndex(
                name: "IX_Component2Project_ProjectID",
                table: "Component2Project",
                column: "ProjectID");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_AssigneeID",
                table: "Ticket",
                column: "AssigneeID");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_ComponentID",
                table: "Ticket",
                column: "ComponentID");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_CreatorID",
                table: "Ticket",
                column: "CreatorID");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_IssueKey",
                table: "Ticket",
                column: "IssueKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_ProjectID",
                table: "Ticket",
                column: "ProjectID");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_StatusID",
                table: "Ticket",
                column: "StatusID");

            migrationBuilder.CreateIndex(
                name: "IX_TicketChange_ChangedByRefId",
                table: "TicketChange",
                column: "ChangedByRefId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketChange_ChangedFieldRefID",
                table: "TicketChange",
                column: "ChangedFieldRefID");

            migrationBuilder.CreateIndex(
                name: "IX_TicketChange_TicketID",
                table: "TicketChange",
                column: "TicketID");

            migrationBuilder.CreateIndex(
                name: "IX_TicketField_ProjectID",
                table: "TicketField",
                column: "ProjectID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserProject");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Component2Project");

            migrationBuilder.DropTable(
                name: "TicketChange");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "TicketField");

            migrationBuilder.DropTable(
                name: "Ticket");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Component");

            migrationBuilder.DropTable(
                name: "Status");

            migrationBuilder.DropTable(
                name: "Project");
        }
    }
}
