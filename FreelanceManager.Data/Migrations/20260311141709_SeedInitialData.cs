using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FreelanceManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "Id", "Address", "CompanyName", "CreatedAt", "Email", "Name", "Notes", "Phone", "Status" },
                values: new object[,]
                {
                    { 1, "123 Main St, New York", "Acme Corp", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "john@acme.com", "John Smith", "Long term client", "123-456-7890", "Active" },
                    { 2, "456 Market St, London", "Globex", new DateTime(2026, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "sara@globex.com", "Sara Jones", "", "987-654-3210", "Active" }
                });

            migrationBuilder.InsertData(
                table: "Invoices",
                columns: new[] { "Id", "ClientId", "CreatedAt", "DueDate", "InvoiceNumber", "IssueDate", "Notes", "Status", "Subtotal", "TaxAmount", "TaxRate", "TotalAmount" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2026, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "INV-0001", new DateTime(2026, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Payment via bank transfer", "Sent", 400m, 60m, 0.15m, 460m },
                    { 2, 2, new DateTime(2026, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 2, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "INV-0002", new DateTime(2026, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "Draft", 1500m, 225m, 0.15m, 1725m }
                });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "BillingType", "ClientId", "CreatedAt", "Description", "EndDate", "FixedPrice", "HourlyRate", "StartDate", "Status", "Title" },
                values: new object[,]
                {
                    { 1, "Hourly", 1, new DateTime(2026, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Build a full e-commerce website", new DateTime(2026, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 50m, new DateTime(2026, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "InProgress", "E-commerce Website" },
                    { 2, "Fixed", 2, new DateTime(2026, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Modern website redesign", new DateTime(2026, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1500m, 0m, new DateTime(2026, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "NotStarted", "Website Redesign" }
                });

            migrationBuilder.InsertData(
                table: "InvoiceItems",
                columns: new[] { "Id", "Description", "InvoiceId", "Quantity", "Total", "UnitPrice" },
                values: new object[,]
                {
                    { 1, "Frontend Development - 8hrs", 1, 1, 400m, 400m },
                    { 2, "Website Redesign - Fixed Price", 2, 1, 1500m, 1500m }
                });

            migrationBuilder.InsertData(
                table: "TimeEntries",
                columns: new[] { "Id", "CreatedAt", "Date", "Description", "HoursWorked", "ProjectId" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Built login page", 5m, 1 },
                    { 2, new DateTime(2026, 1, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "Built navigation bar", 3m, 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "InvoiceItems",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "InvoiceItems",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "TimeEntries",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "TimeEntries",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Invoices",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Invoices",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
