using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderSolution.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class update_orderItem_fk_orderId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Orders_OrderEntityId",
                table: "OrderItems");

            migrationBuilder.DropIndex(
                name: "IX_OrderItems_OrderEntityId",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "OrderEntityId",
                table: "OrderItems");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Orders_OrderId",
                table: "OrderItems",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Orders_OrderId",
                table: "OrderItems");

            migrationBuilder.AddColumn<Guid>(
                name: "OrderEntityId",
                table: "OrderItems",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderEntityId",
                table: "OrderItems",
                column: "OrderEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Orders_OrderEntityId",
                table: "OrderItems",
                column: "OrderEntityId",
                principalTable: "Orders",
                principalColumn: "Id");
        }
    }
}
