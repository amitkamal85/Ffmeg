using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NotFinal.Migrations
{
    public partial class update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "VideoConfigs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "VideoConfigs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Duration",
                table: "VideoConfigs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProcessingTme",
                table: "VideoConfigs",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "VideoConfigs");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "VideoConfigs");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "VideoConfigs");

            migrationBuilder.DropColumn(
                name: "ProcessingTme",
                table: "VideoConfigs");
        }
    }
}
