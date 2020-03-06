using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace deft_pay_backend.Migrations
{
    public partial class Transaction_Token : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TransactionTokens",
                columns: table => new
                {
                    TransactionTokenId = table.Column<string>(nullable: false),
                    Amount = table.Column<double>(nullable: false),
                    OTP = table.Column<string>(nullable: true),
                    TimeCreated = table.Column<DateTime>(nullable: false),
                    ExpiryDate = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionTokens", x => x.TransactionTokenId);
                    table.ForeignKey(
                        name: "FK_TransactionTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TransactionTokens_OTP",
                table: "TransactionTokens",
                column: "OTP",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TransactionTokens_UserId",
                table: "TransactionTokens",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TransactionTokens");
        }
    }
}
