using Microsoft.EntityFrameworkCore.Migrations;

namespace deft_pay_backend.Migrations
{
    public partial class Transaction_Token_IsUsed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsExpired",
                table: "TransactionTokens",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsUsed",
                table: "TransactionTokens",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsExpired",
                table: "TransactionTokens");

            migrationBuilder.DropColumn(
                name: "IsUsed",
                table: "TransactionTokens");
        }
    }
}
