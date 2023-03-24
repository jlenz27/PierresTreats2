﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PierresTreats.Migrations
{
    /// <inheritdoc />
    public partial class UserTreat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Treats",
                type: "varchar(255)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Treats_UserId",
                table: "Treats",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Treats_AspNetUsers_UserId",
                table: "Treats",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Treats_AspNetUsers_UserId",
                table: "Treats");

            migrationBuilder.DropIndex(
                name: "IX_Treats_UserId",
                table: "Treats");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Treats");
        }
    }
}