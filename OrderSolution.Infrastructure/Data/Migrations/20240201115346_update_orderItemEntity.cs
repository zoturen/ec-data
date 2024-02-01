﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderSolution.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class update_orderItemEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ArticleNumber",
                table: "OrderItems",
                type: "text",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "ArticleNumber",
                table: "OrderItems",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
