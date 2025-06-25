using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace grp_management.Migrations
{
    /// <inheritdoc />
    public partial class AddMoreSeedTemplates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: -2,
                column: "AddedAt",
                value: new DateTime(2025, 6, 25, 15, 8, 42, 493, DateTimeKind.Utc).AddTicks(4957));

            migrationBuilder.UpdateData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: -1,
                column: "AddedAt",
                value: new DateTime(2025, 6, 25, 15, 8, 42, 493, DateTimeKind.Utc).AddTicks(4953));

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "EmpNO",
                keyValue: -2,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 25, 15, 8, 41, 850, DateTimeKind.Utc).AddTicks(2454));

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "EmpNO",
                keyValue: -1,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 25, 15, 8, 41, 850, DateTimeKind.Utc).AddTicks(2451));

            migrationBuilder.UpdateData(
                table: "GroupMembershipRequests",
                keyColumn: "RequestId",
                keyValue: -4,
                columns: new[] { "ProcessedDate", "RequestDate" },
                values: new object[] { new DateTime(2025, 6, 23, 15, 8, 41, 850, DateTimeKind.Utc).AddTicks(2757), new DateTime(2025, 6, 22, 15, 8, 41, 850, DateTimeKind.Utc).AddTicks(2756) });

            migrationBuilder.UpdateData(
                table: "GroupMembershipRequests",
                keyColumn: "RequestId",
                keyValue: -3,
                columns: new[] { "ProcessedDate", "RequestDate" },
                values: new object[] { new DateTime(2025, 6, 24, 15, 8, 41, 850, DateTimeKind.Utc).AddTicks(2745), new DateTime(2025, 6, 23, 15, 8, 41, 850, DateTimeKind.Utc).AddTicks(2744) });

            migrationBuilder.UpdateData(
                table: "GroupMembershipRequests",
                keyColumn: "RequestId",
                keyValue: -2,
                column: "RequestDate",
                value: new DateTime(2025, 6, 25, 15, 8, 41, 850, DateTimeKind.Utc).AddTicks(2742));

            migrationBuilder.UpdateData(
                table: "GroupMembershipRequests",
                keyColumn: "RequestId",
                keyValue: -1,
                column: "RequestDate",
                value: new DateTime(2025, 6, 24, 15, 8, 41, 850, DateTimeKind.Utc).AddTicks(2733));

            migrationBuilder.UpdateData(
                table: "Groups",
                keyColumn: "Id",
                keyValue: -2,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 25, 15, 8, 41, 850, DateTimeKind.Utc).AddTicks(2701));

            migrationBuilder.UpdateData(
                table: "Groups",
                keyColumn: "Id",
                keyValue: -1,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 25, 15, 8, 41, 850, DateTimeKind.Utc).AddTicks(2697));

            migrationBuilder.UpdateData(
                table: "SentMsgs",
                keyColumn: "SentMsgID",
                keyValue: -2,
                column: "SentDate",
                value: new DateTime(2025, 6, 25, 10, 8, 42, 493, DateTimeKind.Utc).AddTicks(5192));

            migrationBuilder.UpdateData(
                table: "SentMsgs",
                keyColumn: "SentMsgID",
                keyValue: -1,
                column: "SentDate",
                value: new DateTime(2025, 6, 25, 5, 8, 42, 493, DateTimeKind.Utc).AddTicks(5177));

            migrationBuilder.InsertData(
                table: "Templates",
                columns: new[] { "TemplateID", "Placeholders", "TemplateMsg", "TemplateName", "TemplateType" },
                values: new object[,]
                {
                    { -5, "{}", "Please be advised that there will be scheduled maintenance in the office this weekend. Please save all your work before leaving on Friday.", "Office Maintenance", "General" },
                    { -4, "{}", "Season's Greetings! Wishing you a happy and prosperous holiday season from all of us.", "Holiday Greeting", "Frequent" },
                    { -3, "{\"Name\":\"Recipient Name\",\"ProjectName\":\"Name of the Project\",\"Status\":\"Current Project Status\"}", "Hello {{Name}}, this is an update regarding your project '{{ProjectName}}'. Current Status: {{Status}}.", "Project Update", "General" }
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: -2,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 6, 25, 15, 8, 42, 493, DateTimeKind.Utc).AddTicks(3981), "$2a$12$7WNKH6swUAOzbLFGb4.21uL8o8OyGnhO9H/.OcUYbt1ZcY7Z42Cri" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: -1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 6, 25, 15, 8, 42, 170, DateTimeKind.Utc).AddTicks(4052), "$2a$12$65YoOElI2vWZRxSf6XT1Lu/Ep3OE9a8YCVZ.CVF9vC6S4FoZ3ukLG" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Templates",
                keyColumn: "TemplateID",
                keyValue: -5);

            migrationBuilder.DeleteData(
                table: "Templates",
                keyColumn: "TemplateID",
                keyValue: -4);

            migrationBuilder.DeleteData(
                table: "Templates",
                keyColumn: "TemplateID",
                keyValue: -3);

            migrationBuilder.UpdateData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: -2,
                column: "AddedAt",
                value: new DateTime(2025, 6, 21, 20, 12, 20, 733, DateTimeKind.Utc).AddTicks(2181));

            migrationBuilder.UpdateData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: -1,
                column: "AddedAt",
                value: new DateTime(2025, 6, 21, 20, 12, 20, 733, DateTimeKind.Utc).AddTicks(2175));

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "EmpNO",
                keyValue: -2,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 21, 20, 12, 19, 760, DateTimeKind.Utc).AddTicks(5507));

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "EmpNO",
                keyValue: -1,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 21, 20, 12, 19, 760, DateTimeKind.Utc).AddTicks(5504));

            migrationBuilder.UpdateData(
                table: "GroupMembershipRequests",
                keyColumn: "RequestId",
                keyValue: -4,
                columns: new[] { "ProcessedDate", "RequestDate" },
                values: new object[] { new DateTime(2025, 6, 19, 20, 12, 19, 760, DateTimeKind.Utc).AddTicks(5869), new DateTime(2025, 6, 18, 20, 12, 19, 760, DateTimeKind.Utc).AddTicks(5868) });

            migrationBuilder.UpdateData(
                table: "GroupMembershipRequests",
                keyColumn: "RequestId",
                keyValue: -3,
                columns: new[] { "ProcessedDate", "RequestDate" },
                values: new object[] { new DateTime(2025, 6, 20, 20, 12, 19, 760, DateTimeKind.Utc).AddTicks(5852), new DateTime(2025, 6, 19, 20, 12, 19, 760, DateTimeKind.Utc).AddTicks(5851) });

            migrationBuilder.UpdateData(
                table: "GroupMembershipRequests",
                keyColumn: "RequestId",
                keyValue: -2,
                column: "RequestDate",
                value: new DateTime(2025, 6, 21, 20, 12, 19, 760, DateTimeKind.Utc).AddTicks(5848));

            migrationBuilder.UpdateData(
                table: "GroupMembershipRequests",
                keyColumn: "RequestId",
                keyValue: -1,
                column: "RequestDate",
                value: new DateTime(2025, 6, 20, 20, 12, 19, 760, DateTimeKind.Utc).AddTicks(5839));

            migrationBuilder.UpdateData(
                table: "Groups",
                keyColumn: "Id",
                keyValue: -2,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 21, 20, 12, 19, 760, DateTimeKind.Utc).AddTicks(5797));

            migrationBuilder.UpdateData(
                table: "Groups",
                keyColumn: "Id",
                keyValue: -1,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 21, 20, 12, 19, 760, DateTimeKind.Utc).AddTicks(5792));

            migrationBuilder.UpdateData(
                table: "SentMsgs",
                keyColumn: "SentMsgID",
                keyValue: -2,
                column: "SentDate",
                value: new DateTime(2025, 6, 21, 15, 12, 20, 733, DateTimeKind.Utc).AddTicks(3225));

            migrationBuilder.UpdateData(
                table: "SentMsgs",
                keyColumn: "SentMsgID",
                keyValue: -1,
                column: "SentDate",
                value: new DateTime(2025, 6, 21, 10, 12, 20, 733, DateTimeKind.Utc).AddTicks(3193));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: -2,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 6, 21, 20, 12, 20, 733, DateTimeKind.Utc).AddTicks(552), "$2a$12$lA02SuNwzu/8BuV2dE.97uJpkUPcr9VJvPggsoxPpL.VJZpmLOb5y" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: -1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 6, 21, 20, 12, 20, 245, DateTimeKind.Utc).AddTicks(3832), "$2a$12$YKGAk73u8fGBvR0UxUXulu7vMBNFx.bQ5LLy0kJ8gweWLMfFsZgZC" });
        }
    }
}
