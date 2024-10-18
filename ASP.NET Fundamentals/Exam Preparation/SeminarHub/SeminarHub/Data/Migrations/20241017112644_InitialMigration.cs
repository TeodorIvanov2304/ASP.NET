using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SeminarHub.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Category identifier")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Category name")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Seminars",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Seminar identifier")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Topic = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Seminar topic name"),
                    Lecturer = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false, comment: "Lecturer name"),
                    Details = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false, comment: "Details for the Seminar"),
                    OrganizerId = table.Column<string>(type: "nvarchar(450)", nullable: false, comment: "Organizer identifier"),
                    DateAndTime = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Date and time of the Seminar"),
                    Duration = table.Column<int>(type: "int", nullable: false, comment: "Duration of the Seminar"),
                    CategoryId = table.Column<int>(type: "int", nullable: false, comment: "Category identifier")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seminars", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Seminars_AspNetUsers_OrganizerId",
                        column: x => x.OrganizerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Seminars_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "SeminarsParticipants",
                columns: table => new
                {
                    SeminarId = table.Column<int>(type: "int", nullable: false, comment: "Seminar identifier"),
                    ParticipantId = table.Column<string>(type: "nvarchar(450)", nullable: false, comment: "Participant identifier")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeminarsParticipants", x => new { x.SeminarId, x.ParticipantId });
                    table.ForeignKey(
                        name: "FK_SeminarsParticipants_AspNetUsers_ParticipantId",
                        column: x => x.ParticipantId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_SeminarsParticipants_Seminars_SeminarId",
                        column: x => x.SeminarId,
                        principalTable: "Seminars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Technology & Innovation" },
                    { 2, "Business & Entrepreneurship" },
                    { 3, "Science & Research" },
                    { 4, "Arts & Culture" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Seminars_CategoryId",
                table: "Seminars",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Seminars_OrganizerId",
                table: "Seminars",
                column: "OrganizerId");

            migrationBuilder.CreateIndex(
                name: "IX_SeminarsParticipants_ParticipantId",
                table: "SeminarsParticipants",
                column: "ParticipantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SeminarsParticipants");

            migrationBuilder.DropTable(
                name: "Seminars");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
