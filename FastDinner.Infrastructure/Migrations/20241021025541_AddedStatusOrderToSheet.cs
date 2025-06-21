using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FastDinner.Infrastructure.Migrations
{
    public partial class AddedStatusOrderToSheet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "NeedPrepare",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ProductType",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "GroupedSheet",
                table: "OrderSheet",
                type: "uniqueidentifier",
                nullable: false);

            migrationBuilder.AddColumn<int>(
                name: "PrepareStatus",
                table: "OrderSheet",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NeedPrepare",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProductType",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "GroupedSheet",
                table: "OrderSheet");

            migrationBuilder.DropColumn(
                name: "PrepareStatus",
                table: "OrderSheet");
        }
    }
}
