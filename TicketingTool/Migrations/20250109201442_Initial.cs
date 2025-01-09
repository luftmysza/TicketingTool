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
                name: "Components",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ComponentName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProjectID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Components", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Components_Project_ProjectID",
                        column: x => x.ProjectID,
                        principalTable: "Project",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectUserRole",
                columns: table => new
                {
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(256)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectUserRole", x => new { x.ProjectId, x.UserId });
                    table.ForeignKey(
                        name: "FK_ProjectUserRole_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "UserName",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectUserRole_Project_ProjectId",
                        column: x => x.ProjectId,
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
                    IssueKey = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
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
                    table.UniqueConstraint("AK_Ticket_IssueKey", x => x.IssueKey);
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
                        name: "FK_Ticket_Components_ComponentID",
                        column: x => x.ComponentID,
                        principalTable: "Components",
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
                name: "Comments",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IssueKey = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    AuthorUserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AuthorRefId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Comments_AspNetUsers_AuthorRefId",
                        column: x => x.AuthorRefId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comments_Ticket_IssueKey",
                        column: x => x.IssueKey,
                        principalTable: "Ticket",
                        principalColumn: "IssueKey",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TicketChange",
                columns: table => new
                {
                    ChangeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TicketID = table.Column<int>(type: "int", nullable: false),
                    ChangedFieldId = table.Column<int>(type: "int", nullable: false),
                    ChangedFieldName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OldValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChangedBy = table.Column<string>(type: "nvarchar(256)", nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketChange", x => x.ChangeID);
                    table.ForeignKey(
                        name: "FK_TicketChange_AspNetUsers_ChangedBy",
                        column: x => x.ChangedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "UserName",
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
                    { 1, 0, "4db88bf2-49e3-42d6-b859-a90675a5d893", null, false, false, null, "Admin", null, "X001", "AQAAAAIAAYagAAAAEJGIzrhTUCqs3jKfqbqNqOyAsDeP2HewgHUCy/vqWB/ZwyXskV9mKi6B7S7jviiLvA==", null, false, "0a483bfa-73dd-4e10-8873-fb529b851682", "User", false, "X001" },
                    { 2, 0, "aa97153a-1574-4111-96bb-b3e312206e8e", null, false, false, null, "Ticket", null, "TECH01", "AQAAAAIAAYagAAAAECcH5ztt7b8qDPChCm3B2FFOfy10+m49BPckz0ivxsv/YQtW8uv86aLXEvoSPtMSNg==", null, false, "b41551d3-efa0-4d70-a9d1-f3510fa8a3c8", "Creator", false, "TECH01" },
                    { 3, 0, "fc9c75d4-4ccb-4bb1-94ed-b2c73ebc574c", null, false, false, null, "Job", null, "TECH02", "AQAAAAIAAYagAAAAEBIB/Dfv/vOY4hu/ww2tMTtE3Ma2b+sceO0U0f5r2ZObQ1nu0pd2JAT3Bjiw1FYb6g==", null, false, "f8e2b1b9-2465-44ea-863d-f2217fec1d9f", "User", false, "TECH02" }
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
                table: "Components",
                columns: new[] { "ID", "ComponentName", "ProjectID" },
                values: new object[,]
                {
                    { 1, "Unidentified", 1 },
                    { 2, "User Interface Module", 1 },
                    { 3, "Database Management", 1 },
                    { 4, "API Gateway", 1 },
                    { 5, "Logging Service", 1 },
                    { 6, "Notification System", 1 },
                    { 7, "Payment Processor", 1 },
                    { 8, "Analytics Engine", 1 },
                    { 9, "Reporting Tool", 1 },
                    { 10, "Cache Management", 1 },
                    { 11, "Authentication Service", 1 }
                });

            migrationBuilder.InsertData(
                table: "ProjectUserRole",
                columns: new[] { "ProjectId", "UserId", "RoleId" },
                values: new object[,]
                {
                    { 1, "TECH01", "TECH" },
                    { 1, "TECH02", "TECH" },
                    { 1, "X001", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "Ticket",
                columns: new[] { "ID", "AssigneeID", "ComponentID", "CreatedDate", "CreatorID", "Description", "IssueKey", "LastUpdatedDate", "ProjectID", "ResolvedDate", "StatusID", "Title" },
                values: new object[,]
                {
                    { 1, null, 1, new DateTime(2025, 1, 9, 21, 14, 41, 776, DateTimeKind.Local).AddTicks(4416), "X001", "Lorem ipsum odor amet, consectetuer adipiscing elit. Curabitur duis non dis ligula potenti praesent aenean. Mus etiam ridiculus viverra sed sapien nascetur, turpis tempor sollicitudin. Aptent enim luctus dui; urna per id. Sodales auctor vel accumsan dictumst placerat feugiat lectus curabitur? Quam risus lorem vitae commodo porttitor orci ultrices.", "BSC-1", new DateTime(2025, 1, 9, 21, 14, 41, 778, DateTimeKind.Local).AddTicks(298), 1, null, 1, "Seed Ticket 1" },
                    { 2, null, 1, new DateTime(2025, 1, 9, 21, 14, 41, 778, DateTimeKind.Local).AddTicks(499), "X001", "Lorem ipsum odor amet, consectetuer adipiscing elit. Curabitur duis non dis ligula potenti praesent aenean. Mus etiam ridiculus viverra sed sapien nascetur, turpis tempor sollicitudin. Aptent enim luctus dui; urna per id. Sodales auctor vel accumsan dictumst placerat feugiat lectus curabitur? Quam risus lorem vitae commodo porttitor orci ultrices.", "BSC-2", new DateTime(2025, 1, 9, 21, 14, 41, 778, DateTimeKind.Local).AddTicks(502), 1, null, 1, "Seed Ticket 2" },
                    { 3, null, 1, new DateTime(2025, 1, 9, 21, 14, 41, 778, DateTimeKind.Local).AddTicks(506), "X001", "Lorem ipsum odor amet, consectetuer adipiscing elit. Curabitur duis non dis ligula potenti praesent aenean. Mus etiam ridiculus viverra sed sapien nascetur, turpis tempor sollicitudin. Aptent enim luctus dui; urna per id. Sodales auctor vel accumsan dictumst placerat feugiat lectus curabitur? Quam risus lorem vitae commodo porttitor orci ultrices.", "BSC-3", new DateTime(2025, 1, 9, 21, 14, 41, 778, DateTimeKind.Local).AddTicks(507), 1, null, 1, "Seed Ticket 3" },
                    { 4, null, 1, new DateTime(2025, 1, 9, 21, 14, 41, 778, DateTimeKind.Local).AddTicks(544), "X001", "Lorem ipsum odor amet, consectetuer adipiscing elit. Curabitur duis non dis ligula potenti praesent aenean. Mus etiam ridiculus viverra sed sapien nascetur, turpis tempor sollicitudin. Aptent enim luctus dui; urna per id. Sodales auctor vel accumsan dictumst placerat feugiat lectus curabitur? Quam risus lorem vitae commodo porttitor orci ultrices.", "BSC-4", new DateTime(2025, 1, 9, 21, 14, 41, 778, DateTimeKind.Local).AddTicks(546), 1, null, 1, "Seed Ticket 4" },
                    { 5, null, 1, new DateTime(2025, 1, 9, 21, 14, 41, 778, DateTimeKind.Local).AddTicks(548), "X001", "Lorem ipsum odor amet, consectetuer adipiscing elit. Curabitur duis non dis ligula potenti praesent aenean. Mus etiam ridiculus viverra sed sapien nascetur, turpis tempor sollicitudin. Aptent enim luctus dui; urna per id. Sodales auctor vel accumsan dictumst placerat feugiat lectus curabitur? Quam risus lorem vitae commodo porttitor orci ultrices.", "BSC-5", new DateTime(2025, 1, 9, 21, 14, 41, 778, DateTimeKind.Local).AddTicks(549), 1, null, 1, "Seed Ticket 5" }
                });

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
                name: "IX_Comments_AuthorRefId",
                table: "Comments",
                column: "AuthorRefId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_IssueKey",
                table: "Comments",
                column: "IssueKey");

            migrationBuilder.CreateIndex(
                name: "IX_Components_ProjectID",
                table: "Components",
                column: "ProjectID");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectUserRole_UserId",
                table: "ProjectUserRole",
                column: "UserId");

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
                name: "IX_TicketChange_ChangedBy",
                table: "TicketChange",
                column: "ChangedBy");

            migrationBuilder.CreateIndex(
                name: "IX_TicketChange_TicketID",
                table: "TicketChange",
                column: "TicketID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                name: "Comments");

            migrationBuilder.DropTable(
                name: "ProjectUserRole");

            migrationBuilder.DropTable(
                name: "TicketChange");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Ticket");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Components");

            migrationBuilder.DropTable(
                name: "Status");

            migrationBuilder.DropTable(
                name: "Project");
        }
    }
}
