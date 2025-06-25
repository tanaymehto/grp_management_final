using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace grp_management.Migrations
{
    /// <inheritdoc />
    public partial class ReseedTemplates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: -2,
                column: "AddedAt",
                value: new DateTime(2025, 6, 25, 15, 26, 57, 229, DateTimeKind.Utc).AddTicks(9169));

            migrationBuilder.UpdateData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: -1,
                column: "AddedAt",
                value: new DateTime(2025, 6, 25, 15, 26, 57, 229, DateTimeKind.Utc).AddTicks(9165));

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "EmpNO",
                keyValue: -2,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 25, 15, 26, 56, 588, DateTimeKind.Utc).AddTicks(6200));

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "EmpNO",
                keyValue: -1,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 25, 15, 26, 56, 588, DateTimeKind.Utc).AddTicks(6197));

            migrationBuilder.UpdateData(
                table: "GroupMembershipRequests",
                keyColumn: "RequestId",
                keyValue: -4,
                columns: new[] { "ProcessedDate", "RequestDate" },
                values: new object[] { new DateTime(2025, 6, 23, 15, 26, 56, 588, DateTimeKind.Utc).AddTicks(6461), new DateTime(2025, 6, 22, 15, 26, 56, 588, DateTimeKind.Utc).AddTicks(6461) });

            migrationBuilder.UpdateData(
                table: "GroupMembershipRequests",
                keyColumn: "RequestId",
                keyValue: -3,
                columns: new[] { "ProcessedDate", "RequestDate" },
                values: new object[] { new DateTime(2025, 6, 24, 15, 26, 56, 588, DateTimeKind.Utc).AddTicks(6451), new DateTime(2025, 6, 23, 15, 26, 56, 588, DateTimeKind.Utc).AddTicks(6450) });

            migrationBuilder.UpdateData(
                table: "GroupMembershipRequests",
                keyColumn: "RequestId",
                keyValue: -2,
                column: "RequestDate",
                value: new DateTime(2025, 6, 25, 15, 26, 56, 588, DateTimeKind.Utc).AddTicks(6448));

            migrationBuilder.UpdateData(
                table: "GroupMembershipRequests",
                keyColumn: "RequestId",
                keyValue: -1,
                column: "RequestDate",
                value: new DateTime(2025, 6, 24, 15, 26, 56, 588, DateTimeKind.Utc).AddTicks(6436));

            migrationBuilder.UpdateData(
                table: "Groups",
                keyColumn: "Id",
                keyValue: -2,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 25, 15, 26, 56, 588, DateTimeKind.Utc).AddTicks(6403));

            migrationBuilder.UpdateData(
                table: "Groups",
                keyColumn: "Id",
                keyValue: -1,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 25, 15, 26, 56, 588, DateTimeKind.Utc).AddTicks(6400));

            migrationBuilder.UpdateData(
                table: "SentMsgs",
                keyColumn: "SentMsgID",
                keyValue: -2,
                column: "SentDate",
                value: new DateTime(2025, 6, 25, 10, 26, 57, 229, DateTimeKind.Utc).AddTicks(9341));

            migrationBuilder.UpdateData(
                table: "SentMsgs",
                keyColumn: "SentMsgID",
                keyValue: -1,
                column: "SentDate",
                value: new DateTime(2025, 6, 25, 5, 26, 57, 229, DateTimeKind.Utc).AddTicks(9327));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: -2,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 6, 25, 15, 26, 57, 229, DateTimeKind.Utc).AddTicks(8381), "$2a$12$1KoQonqvVlWSs5bVLmPLN.6JPa./dPB6.0GpuynDHZgwyQlB7.0ea" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: -1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 6, 25, 15, 26, 56, 907, DateTimeKind.Utc).AddTicks(4372), "$2a$12$NCg9W9Tmw.REbESsFxJ.V.Z/wvcnnBc5T24MTbIRomoTngfSYTwoW" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
    }
}
