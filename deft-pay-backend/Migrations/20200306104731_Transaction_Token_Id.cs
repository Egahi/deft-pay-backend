using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace deft_pay_backend.Migrations
{
    public partial class Transaction_Token_Id : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "TransactionTokenId",
                table: "TransactionTokens",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255) CHARACTER SET utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TransactionTokenId",
                table: "TransactionTokens",
                type: "varchar(255) CHARACTER SET utf8mb4",
                nullable: false,
                oldClrType: typeof(Guid));
        }
    }
}
