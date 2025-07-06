using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShortUrl.Repository.Migrations
{
    /// <inheritdoc />
    public partial class EnhancedShortUrlModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add new columns
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "ShortUrls",
                type: "TEXT",
                nullable: false,
                defaultValue: DateTime.UtcNow);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastAccessed",
                table: "ShortUrls",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AccessCount",
                table: "ShortUrls",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ShortUrls",
                type: "INTEGER",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiresAt",
                table: "ShortUrls",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "ShortUrls",
                type: "TEXT",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "ShortUrls",
                type: "TEXT",
                maxLength: 500,
                nullable: true);

            // Modify existing columns for better performance
            migrationBuilder.AlterColumn<string>(
                name: "ShortURL",
                table: "ShortUrls",
                type: "VARCHAR(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "LongURL",
                table: "ShortUrls",
                type: "VARCHAR(2000)",
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            // Create indexes for better performance
            migrationBuilder.CreateIndex(
                name: "IX_ShortUrls_ShortURL",
                table: "ShortUrls",
                column: "ShortURL",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShortUrls_LongURL",
                table: "ShortUrls",
                column: "LongURL");

            migrationBuilder.CreateIndex(
                name: "IX_ShortUrls_CreatedAt",
                table: "ShortUrls",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ShortUrls_IsActive",
                table: "ShortUrls",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_ShortUrls_ExpiresAt",
                table: "ShortUrls",
                column: "ExpiresAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop indexes
            migrationBuilder.DropIndex(
                name: "IX_ShortUrls_ShortURL",
                table: "ShortUrls");

            migrationBuilder.DropIndex(
                name: "IX_ShortUrls_LongURL",
                table: "ShortUrls");

            migrationBuilder.DropIndex(
                name: "IX_ShortUrls_CreatedAt",
                table: "ShortUrls");

            migrationBuilder.DropIndex(
                name: "IX_ShortUrls_IsActive",
                table: "ShortUrls");

            migrationBuilder.DropIndex(
                name: "IX_ShortUrls_ExpiresAt",
                table: "ShortUrls");

            // Drop columns
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "ShortUrls");

            migrationBuilder.DropColumn(
                name: "LastAccessed",
                table: "ShortUrls");

            migrationBuilder.DropColumn(
                name: "AccessCount",
                table: "ShortUrls");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "ShortUrls");

            migrationBuilder.DropColumn(
                name: "ExpiresAt",
                table: "ShortUrls");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ShortUrls");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "ShortUrls");

            // Revert column changes
            migrationBuilder.AlterColumn<string>(
                name: "ShortURL",
                table: "ShortUrls",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "LongURL",
                table: "ShortUrls",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR(2000)",
                oldMaxLength: 2000);
        }
    }
}