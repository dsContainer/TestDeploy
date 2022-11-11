using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Digital_BE.Migrations
{
    public partial class UpdateTemplateId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Template",
                table: "Processes");

            migrationBuilder.AddColumn<Guid>(
                name: "TemplateId",
                table: "Processes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Processes_TemplateId",
                table: "Processes",
                column: "TemplateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Processes_Templates_TemplateId",
                table: "Processes",
                column: "TemplateId",
                principalTable: "Templates",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Processes_Templates_TemplateId",
                table: "Processes");

            migrationBuilder.DropIndex(
                name: "IX_Processes_TemplateId",
                table: "Processes");

            migrationBuilder.DropColumn(
                name: "TemplateId",
                table: "Processes");

            migrationBuilder.AddColumn<string>(
                name: "Template",
                table: "Processes",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
