using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace grp_management.Migrations
{
    /// <inheritdoc />
    public partial class RemoveFailedSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SentMsgs",
                keyColumn: "SentMsgID",
                keyValue: -4);

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
                keyValue: -3,
                column: "SentDate",
                value: new DateTime(2025, 6, 22, 0, 34, 57, 485, DateTimeKind.Local).AddTicks(4134));

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: -2,
                column: "AddedAt",
                value: new DateTime(2025, 6, 21, 20, 4, 57, 485, DateTimeKind.Utc).AddTicks(3934));

            migrationBuilder.UpdateData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: -1,
                column: "AddedAt",
                value: new DateTime(2025, 6, 21, 20, 4, 57, 485, DateTimeKind.Utc).AddTicks(3930));

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "EmpNO",
                keyValue: -2,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 21, 20, 4, 56, 808, DateTimeKind.Utc).AddTicks(8362));

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "EmpNO",
                keyValue: -1,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 21, 20, 4, 56, 808, DateTimeKind.Utc).AddTicks(8359));

            migrationBuilder.UpdateData(
                table: "GroupMembershipRequests",
                keyColumn: "RequestId",
                keyValue: -4,
                columns: new[] { "ProcessedDate", "RequestDate" },
                values: new object[] { new DateTime(2025, 6, 19, 20, 4, 56, 808, DateTimeKind.Utc).AddTicks(8749), new DateTime(2025, 6, 18, 20, 4, 56, 808, DateTimeKind.Utc).AddTicks(8748) });

            migrationBuilder.UpdateData(
                table: "GroupMembershipRequests",
                keyColumn: "RequestId",
                keyValue: -3,
                columns: new[] { "ProcessedDate", "RequestDate" },
                values: new object[] { new DateTime(2025, 6, 20, 20, 4, 56, 808, DateTimeKind.Utc).AddTicks(8742), new DateTime(2025, 6, 19, 20, 4, 56, 808, DateTimeKind.Utc).AddTicks(8741) });

            migrationBuilder.UpdateData(
                table: "GroupMembershipRequests",
                keyColumn: "RequestId",
                keyValue: -2,
                column: "RequestDate",
                value: new DateTime(2025, 6, 21, 20, 4, 56, 808, DateTimeKind.Utc).AddTicks(8734));

            migrationBuilder.UpdateData(
                table: "GroupMembershipRequests",
                keyColumn: "RequestId",
                keyValue: -1,
                column: "RequestDate",
                value: new DateTime(2025, 6, 20, 20, 4, 56, 808, DateTimeKind.Utc).AddTicks(8726));

            migrationBuilder.UpdateData(
                table: "Groups",
                keyColumn: "Id",
                keyValue: -2,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 21, 20, 4, 56, 808, DateTimeKind.Utc).AddTicks(8688));

            migrationBuilder.UpdateData(
                table: "Groups",
                keyColumn: "Id",
                keyValue: -1,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 21, 20, 4, 56, 808, DateTimeKind.Utc).AddTicks(8684));

            migrationBuilder.UpdateData(
                table: "SentMsgs",
                keyColumn: "SentMsgID",
                keyValue: -3,
                column: "SentDate",
                value: new DateTime(2025, 6, 21, 19, 4, 57, 485, DateTimeKind.Utc).AddTicks(4134));

            migrationBuilder.UpdateData(
                table: "SentMsgs",
                keyColumn: "SentMsgID",
                keyValue: -2,
                column: "SentDate",
                value: new DateTime(2025, 6, 21, 15, 4, 57, 485, DateTimeKind.Utc).AddTicks(4131));

            migrationBuilder.UpdateData(
                table: "SentMsgs",
                keyColumn: "SentMsgID",
                keyValue: -1,
                column: "SentDate",
                value: new DateTime(2025, 6, 21, 10, 4, 57, 485, DateTimeKind.Utc).AddTicks(4118));

            migrationBuilder.InsertData(
                table: "SentMsgs",
                columns: new[] { "SentMsgID", "GroupId", "MessageContent", "SenderEmployeeId", "SentDate", "SentVia", "Status", "TemplateID", "VariablesJson" },
                values: new object[] { -4, -2, "A final test message for a specific group.", -2, new DateTime(2025, 6, 21, 20, 4, 57, 485, DateTimeKind.Utc).AddTicks(4136), "API", "Failed", -2, "{}" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: -2,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 6, 21, 20, 4, 57, 485, DateTimeKind.Utc).AddTicks(2985), "$2a$12$ea2UMYUhGkglJDy65FG2H.1GkbO2M2l98w7HVGWolku06149btBsK" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: -1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 6, 21, 20, 4, 57, 148, DateTimeKind.Utc).AddTicks(4316), "$2a$12$mFYE6DUT/VwQCYnREusY9e74x6Cd3HeTK8VNM7BsjiliAPjLHLmCS" });
        }
    }
}
