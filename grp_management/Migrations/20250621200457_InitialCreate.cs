using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace grp_management.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    EmpNO = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Department = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Position = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.EmpNO);
                });

            migrationBuilder.CreateTable(
                name: "MessageTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Placeholders = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageTemplates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Templates",
                columns: table => new
                {
                    TemplateID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TemplateName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TemplateMsg = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TemplateType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Placeholders = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Templates", x => x.TemplateID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsPrivate = table.Column<bool>(type: "bit", nullable: false),
                    MaxMembers = table.Column<int>(type: "int", nullable: true),
                    DepartmentRestriction = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Groups_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AccessLevel = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Member"),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contacts_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmpNO");
                    table.ForeignKey(
                        name: "FK_Contacts_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Contacts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GroupEmployees",
                columns: table => new
                {
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupEmployees", x => new { x.GroupId, x.EmployeeId });
                    table.ForeignKey(
                        name: "FK_GroupEmployees_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmpNO",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupEmployees_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GroupMembershipRequests",
                columns: table => new
                {
                    RequestId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Pending"),
                    AdminComments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProcessedByUserId = table.Column<int>(type: "int", nullable: true),
                    ProcessedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupMembershipRequests", x => x.RequestId);
                    table.ForeignKey(
                        name: "FK_GroupMembershipRequests_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmpNO",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupMembershipRequests_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupMembershipRequests_Users_ProcessedByUserId",
                        column: x => x.ProcessedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    SentVia = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MessageContent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SentAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SentMsgs",
                columns: table => new
                {
                    SentMsgID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MessageContent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SentVia = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    TemplateID = table.Column<int>(type: "int", nullable: true),
                    VariablesJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SenderEmployeeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SentMsgs", x => x.SentMsgID);
                    table.ForeignKey(
                        name: "FK_SentMsgs_Employees_SenderEmployeeId",
                        column: x => x.SenderEmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmpNO",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SentMsgs_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SentMsgs_Templates_TemplateID",
                        column: x => x.TemplateID,
                        principalTable: "Templates",
                        principalColumn: "TemplateID");
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "EmpNO", "CreatedAt", "Department", "Email", "Name", "PhoneNumber", "Position", "UpdatedAt" },
                values: new object[,]
                {
                    { -2, new DateTime(2025, 6, 21, 20, 4, 56, 808, DateTimeKind.Utc).AddTicks(8362), "HR", "test.employee2@example.com", "Test Employee 2", null, "Assistant", null },
                    { -1, new DateTime(2025, 6, 21, 20, 4, 56, 808, DateTimeKind.Utc).AddTicks(8359), "IT", "test.employee1@example.com", "Test Employee 1", null, "Developer", null }
                });

            migrationBuilder.InsertData(
                table: "Groups",
                columns: new[] { "Id", "CreatedAt", "CreatedByUserId", "DepartmentRestriction", "Description", "IsPrivate", "MaxMembers", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { -2, new DateTime(2025, 6, 21, 20, 4, 56, 808, DateTimeKind.Utc).AddTicks(8688), null, "HR", "Another test group", false, 20, "Test Group 2", null },
                    { -1, new DateTime(2025, 6, 21, 20, 4, 56, 808, DateTimeKind.Utc).AddTicks(8684), null, "IT", "A test group for membership requests", true, 10, "Test Group 1", null }
                });

            migrationBuilder.InsertData(
                table: "Templates",
                columns: new[] { "TemplateID", "Placeholders", "TemplateMsg", "TemplateName", "TemplateType" },
                values: new object[,]
                {
                    { -2, "{}", "Reminder: Meeting at 11 AM. See you there!", "Meeting Reminder", "Frequent" },
                    { -1, "{\"Name\":\"Recipient Name\"}", "Happy Birthday, {{Name}}! Wishing you a wonderful day.", "Happy Birthday", "General" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "PasswordHash", "Role", "Username" },
                values: new object[,]
                {
                    { -2, new DateTime(2025, 6, 21, 20, 4, 57, 485, DateTimeKind.Utc).AddTicks(2985), "user@example.com", "$2a$12$ea2UMYUhGkglJDy65FG2H.1GkbO2M2l98w7HVGWolku06149btBsK", "User", "user" },
                    { -1, new DateTime(2025, 6, 21, 20, 4, 57, 148, DateTimeKind.Utc).AddTicks(4316), "admin@example.com", "$2a$12$mFYE6DUT/VwQCYnREusY9e74x6Cd3HeTK8VNM7BsjiliAPjLHLmCS", "Admin", "admin" }
                });

            migrationBuilder.InsertData(
                table: "Contacts",
                columns: new[] { "Id", "AccessLevel", "AddedAt", "EmployeeId", "GroupId", "UserId", "UserName" },
                values: new object[,]
                {
                    { -2, "Member", new DateTime(2025, 6, 21, 20, 4, 57, 485, DateTimeKind.Utc).AddTicks(3934), -2, -2, -2, "testuser2" },
                    { -1, "Admin", new DateTime(2025, 6, 21, 20, 4, 57, 485, DateTimeKind.Utc).AddTicks(3930), -1, -1, -1, "testuser1" }
                });

            migrationBuilder.InsertData(
                table: "GroupMembershipRequests",
                columns: new[] { "RequestId", "AdminComments", "EmployeeId", "GroupId", "ProcessedByUserId", "ProcessedDate", "RequestDate", "Status" },
                values: new object[,]
                {
                    { -4, "Did not meet department requirements.", -1, -2, null, new DateTime(2025, 6, 19, 20, 4, 56, 808, DateTimeKind.Utc).AddTicks(8749), new DateTime(2025, 6, 18, 20, 4, 56, 808, DateTimeKind.Utc).AddTicks(8748), "Rejected" },
                    { -3, null, -2, -1, null, new DateTime(2025, 6, 20, 20, 4, 56, 808, DateTimeKind.Utc).AddTicks(8742), new DateTime(2025, 6, 19, 20, 4, 56, 808, DateTimeKind.Utc).AddTicks(8741), "Approved" },
                    { -2, null, -2, -2, null, null, new DateTime(2025, 6, 21, 20, 4, 56, 808, DateTimeKind.Utc).AddTicks(8734), "Pending" },
                    { -1, null, -1, -1, null, null, new DateTime(2025, 6, 20, 20, 4, 56, 808, DateTimeKind.Utc).AddTicks(8726), "Pending" }
                });

            migrationBuilder.InsertData(
                table: "SentMsgs",
                columns: new[] { "SentMsgID", "GroupId", "MessageContent", "SenderEmployeeId", "SentDate", "SentVia", "Status", "TemplateID", "VariablesJson" },
                values: new object[,]
                {
                    { -4, -2, "A final test message for a specific group.", -2, new DateTime(2025, 6, 21, 20, 4, 57, 485, DateTimeKind.Utc).AddTicks(4136), "API", "Failed", -2, "{}" },
                    { -3, -1, "A third test message for all.", -1, new DateTime(2025, 6, 21, 19, 4, 57, 485, DateTimeKind.Utc).AddTicks(4134), "API", "Sent", -1, "{}" },
                    { -2, -2, "This is another test message.", -2, new DateTime(2025, 6, 21, 15, 4, 57, 485, DateTimeKind.Utc).AddTicks(4131), "WebApp", "Sent", -2, "{}" },
                    { -1, -1, "This is a test message.", -1, new DateTime(2025, 6, 21, 10, 4, 57, 485, DateTimeKind.Utc).AddTicks(4118), "WebApp", "Sent", -1, "{}" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_EmployeeId",
                table: "Contacts",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_GroupId",
                table: "Contacts",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_UserId",
                table: "Contacts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_Email",
                table: "Employees",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GroupEmployees_EmployeeId",
                table: "GroupEmployees",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMembershipRequests_EmployeeId",
                table: "GroupMembershipRequests",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMembershipRequests_GroupId",
                table: "GroupMembershipRequests",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMembershipRequests_ProcessedByUserId",
                table: "GroupMembershipRequests",
                column: "ProcessedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_CreatedByUserId",
                table: "Groups",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_Name",
                table: "Groups",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Messages_GroupId",
                table: "Messages",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_SentMsgs_GroupId",
                table: "SentMsgs",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_SentMsgs_SenderEmployeeId",
                table: "SentMsgs",
                column: "SenderEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_SentMsgs_TemplateID",
                table: "SentMsgs",
                column: "TemplateID");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contacts");

            migrationBuilder.DropTable(
                name: "GroupEmployees");

            migrationBuilder.DropTable(
                name: "GroupMembershipRequests");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "MessageTemplates");

            migrationBuilder.DropTable(
                name: "SentMsgs");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "Templates");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
