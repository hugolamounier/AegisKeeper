using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AegisKeeper.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DatabaseServers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false, defaultValueSql: "NEWID()"),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Provider = table.Column<int>(type: "INTEGER", nullable: false),
                    ConnectionString = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "getutcdate()"),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DatabaseServers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StorageProviders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false, defaultValueSql: "NEWID()"),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Provider = table.Column<int>(type: "INTEGER", nullable: false),
                    StorageName = table.Column<string>(type: "TEXT", nullable: false),
                    ContainerName = table.Column<string>(type: "TEXT", nullable: false),
                    Credential = table.Column<string>(type: "TEXT", nullable: true),
                    ConnectionString = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "getutcdate()"),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StorageProviders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Backups",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false, defaultValueSql: "NEWID()"),
                    DatabaseServerId = table.Column<Guid>(type: "TEXT", nullable: false),
                    StorageProviderId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CurrentStep = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    Database = table.Column<string>(type: "TEXT", nullable: false),
                    Failed = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "getutcdate()"),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Backups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Backups_DatabaseServers_DatabaseServerId",
                        column: x => x.DatabaseServerId,
                        principalTable: "DatabaseServers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Backups_StorageProviders_StorageProviderId",
                        column: x => x.StorageProviderId,
                        principalTable: "StorageProviders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Backups_DatabaseServerId",
                table: "Backups",
                column: "DatabaseServerId");

            migrationBuilder.CreateIndex(
                name: "IX_Backups_StorageProviderId",
                table: "Backups",
                column: "StorageProviderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Backups");

            migrationBuilder.DropTable(
                name: "DatabaseServers");

            migrationBuilder.DropTable(
                name: "StorageProviders");
        }
    }
}
