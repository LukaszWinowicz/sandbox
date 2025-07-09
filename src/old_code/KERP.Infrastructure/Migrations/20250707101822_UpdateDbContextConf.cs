using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDbContextConf : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ValidationPurchaseOrders",
                table: "ValidationPurchaseOrders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PurchaseOrderReceiptDateUpdates",
                table: "PurchaseOrderReceiptDateUpdates");

            migrationBuilder.EnsureSchema(
                name: "upd");

            migrationBuilder.EnsureSchema(
                name: "bgq");

            migrationBuilder.RenameTable(
                name: "ValidationPurchaseOrders",
                newName: "PurchaseOrders",
                newSchema: "bgq");

            migrationBuilder.RenameTable(
                name: "PurchaseOrderReceiptDateUpdates",
                newName: "PurchaseOrderReceiptDate",
                newSchema: "upd");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PurchaseOrders",
                schema: "bgq",
                table: "PurchaseOrders",
                columns: new[] { "PurchaseOrder", "LineNumber", "Sequence" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_PurchaseOrderReceiptDate",
                schema: "upd",
                table: "PurchaseOrderReceiptDate",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PurchaseOrders",
                schema: "bgq",
                table: "PurchaseOrders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PurchaseOrderReceiptDate",
                schema: "upd",
                table: "PurchaseOrderReceiptDate");

            migrationBuilder.RenameTable(
                name: "PurchaseOrders",
                schema: "bgq",
                newName: "ValidationPurchaseOrders");

            migrationBuilder.RenameTable(
                name: "PurchaseOrderReceiptDate",
                schema: "upd",
                newName: "PurchaseOrderReceiptDateUpdates");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ValidationPurchaseOrders",
                table: "ValidationPurchaseOrders",
                columns: new[] { "PurchaseOrder", "LineNumber", "Sequence" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_PurchaseOrderReceiptDateUpdates",
                table: "PurchaseOrderReceiptDateUpdates",
                column: "Id");
        }
    }
}
