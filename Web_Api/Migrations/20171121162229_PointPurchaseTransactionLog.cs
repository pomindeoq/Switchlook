using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WebApi.Migrations
{
    public partial class PointPurchaseTransactionLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PointPurchaseTransactionLog",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AccountId = table.Column<string>(nullable: true),
                    DateTime = table.Column<DateTime>(nullable: false),
                    Price = table.Column<double>(nullable: false),
                    TransactionId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PointPurchaseTransactionLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PointPurchaseTransactionLog_AspNetUsers_AccountId",
                        column: x => x.AccountId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PointPurchaseTransactionLog_PointTransactionLog_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "PointTransactionLog",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PointPurchaseTransactionLog_AccountId",
                table: "PointPurchaseTransactionLog",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_PointPurchaseTransactionLog_TransactionId",
                table: "PointPurchaseTransactionLog",
                column: "TransactionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PointPurchaseTransactionLog");
        }
    }
}
