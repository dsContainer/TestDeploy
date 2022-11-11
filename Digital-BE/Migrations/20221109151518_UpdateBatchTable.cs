using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Digital_BE.Migrations
{
    public partial class UpdateBatchTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Processes_Batches_BatchId1",
                table: "Processes");

            migrationBuilder.DropIndex(
                name: "IX_Processes_BatchId1",
                table: "Processes");

            migrationBuilder.DropColumn(
                name: "BatchId1",
                table: "Processes");

            migrationBuilder.AlterColumn<Guid>(
                name: "BatchId",
                table: "Processes",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ProcessDatas",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "DateUpload",
                table: "ProcessDatas",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "ProcessId",
                table: "Batches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Processes_BatchId",
                table: "Processes",
                column: "BatchId");

            migrationBuilder.AddForeignKey(
                name: "FK_Processes_Batches_BatchId",
                table: "Processes",
                column: "BatchId",
                principalTable: "Batches",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Processes_Batches_BatchId",
                table: "Processes");

            migrationBuilder.DropIndex(
                name: "IX_Processes_BatchId",
                table: "Processes");

            migrationBuilder.DropColumn(
                name: "ProcessId",
                table: "Batches");

            migrationBuilder.AlterColumn<string>(
                name: "BatchId",
                table: "Processes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "BatchId1",
                table: "Processes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ProcessDatas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DateUpload",
                table: "ProcessDatas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Processes_BatchId1",
                table: "Processes",
                column: "BatchId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Processes_Batches_BatchId1",
                table: "Processes",
                column: "BatchId1",
                principalTable: "Batches",
                principalColumn: "Id");
        }
    }
}
