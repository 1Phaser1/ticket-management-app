using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FullName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    Status = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Priority = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    AssignedToId = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tickets_Users_AssignedToId",
                        column: x => x.AssignedToId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TicketId = table.Column<int>(type: "integer", nullable: false),
                    Text = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_Tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Tickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Tickets",
                columns: new[] { "Id", "AssignedToId", "CreatedAt", "Description", "Priority", "Status", "Title", "UpdatedAt" },
                values: new object[] { 5, null, new DateTime(2026, 5, 24, 14, 0, 0, 0, DateTimeKind.Utc), "Destek talepleri icin kategori secimi eklenmesi isteniyor.", "Medium", "Open", "Yeni kategori ekleme", null });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "FullName" },
                values: new object[,]
                {
                    { 1, "ayse.demir@example.com", "Ayse Demir" },
                    { 2, "mehmet.yilmaz@example.com", "Mehmet Yilmaz" },
                    { 3, "zeynep.kaya@example.com", "Zeynep Kaya" }
                });

            migrationBuilder.InsertData(
                table: "Tickets",
                columns: new[] { "Id", "AssignedToId", "CreatedAt", "Description", "Priority", "Status", "Title", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2026, 5, 24, 9, 0, 0, 0, DateTimeKind.Utc), "Kullanici dogru sifre ile giris yapmasina ragmen hata aliyor.", "High", "Open", "Login ekraninda hata", null },
                    { 2, 2, new DateTime(2026, 5, 24, 10, 0, 0, 0, DateTimeKind.Utc), "Aylik talep raporlarinin Excel olarak indirilebilmesi gerekiyor.", "Medium", "In Progress", "Rapor disari aktarma istegi", new DateTime(2026, 5, 24, 12, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, 3, new DateTime(2026, 5, 24, 11, 0, 0, 0, DateTimeKind.Utc), "Yeni talep acildiginda sorumlu kullaniciya e-posta ulasmiyor.", "High", "Open", "Bildirim e-postalari gelmiyor", null },
                    { 4, 1, new DateTime(2026, 5, 24, 13, 0, 0, 0, DateTimeKind.Utc), "Profil detaylari yuklenirken bekleme suresi cok uzun.", "Low", "Resolved", "Profil sayfasi yavas aciliyor", new DateTime(2026, 5, 24, 17, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "Comments",
                columns: new[] { "Id", "CreatedAt", "Text", "TicketId" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 5, 24, 9, 30, 0, 0, DateTimeKind.Utc), "Hata tekrar uretildi, loglar inceleniyor.", 1 },
                    { 2, new DateTime(2026, 5, 24, 11, 0, 0, 0, DateTimeKind.Utc), "Excel export icin gerekli alanlar belirlendi.", 2 },
                    { 3, new DateTime(2026, 5, 24, 16, 0, 0, 0, DateTimeKind.Utc), "Sorgu optimizasyonu sonrasi performans iyilesti.", 4 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_TicketId",
                table: "Comments",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_AssignedToId",
                table: "Tickets",
                column: "AssignedToId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
