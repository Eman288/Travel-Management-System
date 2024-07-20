using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVCPro.Migrations
{
    public partial class TripApp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "buses",
                columns: table => new
                {
                    BusID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumberOfSeats = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_buses", x => x.BusID);
                });

            migrationBuilder.CreateTable(
                name: "staff",
                columns: table => new
                {
                    NationalId = table.Column<string>(type: "varchar(14)", maxLength: 14, nullable: false),
                    Name = table.Column<string>(type: "varchar(250)", nullable: false),
                    ImageUser = table.Column<string>(type: "varchar(250)", nullable: false),
                    Email = table.Column<string>(type: "varchar(250)", nullable: false),
                    Type = table.Column<string>(type: "varchar(250)", nullable: false),
                    Birthday = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StaffCode = table.Column<string>(type: "varchar(250)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_staff", x => x.NationalId);
                });

            migrationBuilder.CreateTable(
                name: "tourists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(250)", nullable: false),
                    City = table.Column<string>(type: "varchar(100)", nullable: false),
                    Description = table.Column<string>(type: "varchar(1000)", nullable: false),
                    Picture = table.Column<string>(type: "varchar(250)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tourists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    NationalId = table.Column<string>(type: "varchar(14)", maxLength: 14, nullable: false),
                    UserName = table.Column<string>(type: "varchar(250)", nullable: false),
                    ImageUser = table.Column<string>(type: "varchar(250)", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Job = table.Column<string>(type: "varchar(250)", nullable: true),
                    Nationality = table.Column<string>(type: "varchar(250)", nullable: false),
                    Email = table.Column<string>(type: "varchar(250)", nullable: false),
                    Password = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.NationalId);
                });

            migrationBuilder.CreateTable(
                name: "trips",
                columns: table => new
                {
                    TripId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(250)", nullable: false),
                    FromLocation = table.Column<string>(type: "varchar(250)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    City = table.Column<string>(type: "varchar(250)", nullable: false),
                    Price = table.Column<string>(type: "varchar(250)", nullable: false),
                    Description = table.Column<string>(type: "varchar(1000)", nullable: false),
                    ImageUser = table.Column<string>(type: "varchar(250)", nullable: false),
                    BusID = table.Column<int>(type: "int", nullable: false),
                    staff = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StaffNationalId = table.Column<string>(type: "varchar(14)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_trips", x => x.TripId);
                    table.ForeignKey(
                        name: "FK_trips_buses_BusID",
                        column: x => x.BusID,
                        principalTable: "buses",
                        principalColumn: "BusID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_trips_staff_StaffNationalId",
                        column: x => x.StaffNationalId,
                        principalTable: "staff",
                        principalColumn: "NationalId");
                });

            migrationBuilder.CreateTable(
                name: "tripatt",
                columns: table => new
                {
                    TripId = table.Column<int>(type: "int", nullable: false),
                    TouristAttractionId = table.Column<int>(type: "int", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tripatt", x => new { x.TripId, x.TouristAttractionId, x.Order });
                    table.ForeignKey(
                        name: "FK_tripatt_tourists_TouristAttractionId",
                        column: x => x.TouristAttractionId,
                        principalTable: "tourists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tripatt_trips_TripId",
                        column: x => x.TripId,
                        principalTable: "trips",
                        principalColumn: "TripId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "usertrips",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(14)", nullable: false),
                    TripId = table.Column<int>(type: "int", nullable: false),
                    BookDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usertrips", x => new { x.TripId, x.UserId });
                    table.ForeignKey(
                        name: "FK_usertrips_trips_TripId",
                        column: x => x.TripId,
                        principalTable: "trips",
                        principalColumn: "TripId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_usertrips_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "NationalId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tripatt_TouristAttractionId",
                table: "tripatt",
                column: "TouristAttractionId");

            migrationBuilder.CreateIndex(
                name: "IX_trips_BusID",
                table: "trips",
                column: "BusID");

            migrationBuilder.CreateIndex(
                name: "IX_trips_StaffNationalId",
                table: "trips",
                column: "StaffNationalId");

            migrationBuilder.CreateIndex(
                name: "IX_usertrips_UserId",
                table: "usertrips",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tripatt");

            migrationBuilder.DropTable(
                name: "usertrips");

            migrationBuilder.DropTable(
                name: "tourists");

            migrationBuilder.DropTable(
                name: "trips");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "buses");

            migrationBuilder.DropTable(
                name: "staff");
        }
    }
}
