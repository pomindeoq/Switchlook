using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WebApi.Migrations
{
    public partial class itemexchangetrans : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ItemExchangeLog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ItemId = table.Column<int>(type: "int", nullable: true),
                    NewOwnerAccountId = table.Column<string>(type: "varchar(127)", nullable: true),
                    OldOwnerAccountId = table.Column<string>(type: "varchar(127)", nullable: true),
                    PointValue = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemExchangeLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemExchangeLog_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ItemExchangeLog_AspNetUsers_NewOwnerAccountId",
                        column: x => x.NewOwnerAccountId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ItemExchangeLog_AspNetUsers_OldOwnerAccountId",
                        column: x => x.OldOwnerAccountId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ItemExchangeLog_ItemId",
                table: "ItemExchangeLog",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemExchangeLog_NewOwnerAccountId",
                table: "ItemExchangeLog",
                column: "NewOwnerAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemExchangeLog_OldOwnerAccountId",
                table: "ItemExchangeLog",
                column: "OldOwnerAccountId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemExchangeLog");
        }
    }
}
