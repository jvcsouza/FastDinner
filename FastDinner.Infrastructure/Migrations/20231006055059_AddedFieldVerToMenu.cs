using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FastDinner.Infrastructure.Migrations
{
    public partial class AddedFieldVerToMenu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Ver",
                table: "Menus",
                type: "rowversion",
                rowVersion: true,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ver",
                table: "Menus");
        }
    }
}
