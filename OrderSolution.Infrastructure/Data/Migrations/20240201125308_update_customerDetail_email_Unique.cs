using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderSolution.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class update_customerDetail_email_Unique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_CustomerDetails_Email",
                table: "CustomerDetails",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CustomerDetails_Email",
                table: "CustomerDetails");
        }
    }
}
