using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JustGo.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Blog",
                columns: table => new
                {
                    BlogID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Describe = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    ImageName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Like = table.Column<int>(type: "int", nullable: true),
                    StartDate = table.Column<DateTime>(type: "date", nullable: false),
                    EndDate = table.Column<DateTime>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blog", x => x.BlogID);
                });

            migrationBuilder.CreateTable(
                name: "Place",
                columns: table => new
                {
                    PlaceID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Tel = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    Add = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    lat = table.Column<decimal>(type: "decimal(9,7)", nullable: false),
                    lng = table.Column<decimal>(type: "decimal(9,6)", nullable: false),
                    Region = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    Town = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    Class = table.Column<int>(type: "int", nullable: false),
                    Opentime = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    intOpentime = table.Column<int>(type: "int", nullable: true),
                    Closetime = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    intClosetime = table.Column<int>(type: "int", nullable: true),
                    Timestay = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Place", x => x.PlaceID);
                });

            migrationBuilder.CreateTable(
                name: "Schedule",
                columns: table => new
                {
                    ScheduleID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StartDate = table.Column<DateTime>(type: "date", nullable: false),
                    EndDate = table.Column<DateTime>(type: "date", nullable: false),
                    WeatherWarning = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedule", x => x.ScheduleID);
                });

            migrationBuilder.CreateTable(
                name: "UserKeep",
                columns: table => new
                {
                    KeepID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    KeepClass = table.Column<int>(type: "int", nullable: false),
                    KeepNumber = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KeepID", x => x.KeepID);
                });

            migrationBuilder.CreateTable(
                name: "weather",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    location = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    locationsName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    startTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    endTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    pop12h = table.Column<int>(type: "int", nullable: true),
                    minT = table.Column<int>(type: "int", nullable: true),
                    maxT = table.Column<int>(type: "int", nullable: true),
                    uvi = table.Column<int>(type: "int", nullable: true),
                    wx = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_weather", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "BlogDetails",
                columns: table => new
                {
                    DetailsID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    EndtTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    PlaceID = table.Column<int>(type: "int", nullable: false),
                    Describe = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Images = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Score = table.Column<double>(type: "float", nullable: true),
                    BlogID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetailsID", x => x.DetailsID);
                    table.ForeignKey(
                        name: "FK_BlogDetails_Blog",
                        column: x => x.BlogID,
                        principalTable: "Blog",
                        principalColumn: "BlogID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ScheduleDetails",
                columns: table => new
                {
                    DetailsID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ScheduleID = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    EndtTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    PlaceID = table.Column<int>(type: "int", nullable: false),
                    Town = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    WeatherWarning = table.Column<bool>(type: "bit", nullable: true),
                    Pop = table.Column<int>(type: "int", nullable: true),
                    Temperature = table.Column<int>(type: "int", nullable: true),
                    Uvi = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeatisID", x => x.DetailsID);
                    table.ForeignKey(
                        name: "FK_ScheduleDetails_Schedule",
                        column: x => x.ScheduleID,
                        principalTable: "Schedule",
                        principalColumn: "ScheduleID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlogDetails_BlogID",
                table: "BlogDetails",
                column: "BlogID");

            migrationBuilder.CreateIndex(
                name: "IX_Place",
                table: "Place",
                columns: new[] { "lat", "lng", "Class", "Region" });

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleDetails_ScheduleID",
                table: "ScheduleDetails",
                column: "ScheduleID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlogDetails");

            migrationBuilder.DropTable(
                name: "Place");

            migrationBuilder.DropTable(
                name: "ScheduleDetails");

            migrationBuilder.DropTable(
                name: "UserKeep");

            migrationBuilder.DropTable(
                name: "weather");

            migrationBuilder.DropTable(
                name: "Blog");

            migrationBuilder.DropTable(
                name: "Schedule");
        }
    }
}
