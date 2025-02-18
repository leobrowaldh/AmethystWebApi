using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DbContext.Migrations.SqlServerDbContext
{
    /// <inheritdoc />
    public partial class miInitial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "supusr");

            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "Addresses",
                schema: "supusr",
                columns: table => new
                {
                    AddressId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    strCity = table.Column<string>(type: "nvarchar(200)", nullable: false),
                    strCountry = table.Column<string>(type: "nvarchar(200)", nullable: false),
                    StreetName = table.Column<string>(type: "nvarchar(200)", nullable: false),
                    City = table.Column<int>(type: "int", nullable: false),
                    Country = table.Column<int>(type: "int", nullable: false),
                    ZipCode = table.Column<int>(type: "int", nullable: false),
                    Seeded = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.AddressId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "dbo",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(200)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(200)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(200)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(200)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Attractions",
                schema: "supusr",
                columns: table => new
                {
                    AttractionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 100, nullable: false),
                    strCategory = table.Column<string>(type: "nvarchar(200)", nullable: true),
                    AddressDbMAddressId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Category = table.Column<int>(type: "int", nullable: false),
                    Seeded = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attractions", x => x.AttractionId);
                    table.ForeignKey(
                        name: "FK_Attractions_Addresses_AddressDbMAddressId",
                        column: x => x.AddressDbMAddressId,
                        principalSchema: "supusr",
                        principalTable: "Addresses",
                        principalColumn: "AddressId");
                });

            migrationBuilder.CreateTable(
                name: "Banks",
                schema: "supusr",
                columns: table => new
                {
                    BankId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    strBank = table.Column<string>(type: "nvarchar(200)", nullable: true),
                    strRiskLevel = table.Column<string>(type: "nvarchar(200)", nullable: true),
                    AttractionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Banks = table.Column<int>(type: "int", nullable: false),
                    BankNumber = table.Column<string>(type: "nvarchar(200)", nullable: true),
                    RiskLevel = table.Column<int>(type: "int", nullable: false),
                    BankComment = table.Column<string>(type: "nvarchar(200)", nullable: true),
                    EncryptedToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Seeded = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banks", x => x.BankId);
                    table.ForeignKey(
                        name: "FK_Banks_Attractions_AttractionId",
                        column: x => x.AttractionId,
                        principalSchema: "supusr",
                        principalTable: "Attractions",
                        principalColumn: "AttractionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                schema: "supusr",
                columns: table => new
                {
                    CommentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    strType = table.Column<string>(type: "nvarchar(200)", nullable: true),
                    strRating = table.Column<string>(type: "nvarchar(200)", nullable: true),
                    AttractionDbMAttractionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    CommentAge = table.Column<int>(type: "int", nullable: false),
                    CommentName = table.Column<string>(type: "nvarchar(200)", nullable: true),
                    CommentText = table.Column<string>(type: "nvarchar(200)", nullable: true),
                    Seeded = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.CommentId);
                    table.ForeignKey(
                        name: "FK_Comments_Attractions_AttractionDbMAttractionId",
                        column: x => x.AttractionDbMAttractionId,
                        principalSchema: "supusr",
                        principalTable: "Attractions",
                        principalColumn: "AttractionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attractions_AddressDbMAddressId",
                schema: "supusr",
                table: "Attractions",
                column: "AddressDbMAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Banks_AttractionId",
                schema: "supusr",
                table: "Banks",
                column: "AttractionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_AttractionDbMAttractionId",
                schema: "supusr",
                table: "Comments",
                column: "AttractionDbMAttractionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Banks",
                schema: "supusr");

            migrationBuilder.DropTable(
                name: "Comments",
                schema: "supusr");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Attractions",
                schema: "supusr");

            migrationBuilder.DropTable(
                name: "Addresses",
                schema: "supusr");
        }
    }
}
